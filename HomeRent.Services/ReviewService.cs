using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Review;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Dashboard;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace HomeRent.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Data.Models.Entities.Review> reviewRepository;

        public ReviewService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Data.Models.Entities.Review> reviewRepository)
        {
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
            this.reviewRepository = reviewRepository;
            
        }

        public async Task<bool> CreateReviewAsync(ReviewCreateDto reviewDto, Guid tenantId)
        {
            var property = await this.propertyRepository.All()
                .FirstAsync(p => p.Id == new Guid(reviewDto.PropertyId));

            if (property == null)
            {
                return false;
            }

            var review = this.mapper.Map<Data.Models.Entities.Review>(reviewDto);
            review.TenantId = tenantId;

            property.Reviews.Add(review);
            
            await this.propertyRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReviewViewModel>> GetPropertyReviewsAsync(Guid propertyId)
        {
            var reviews = await this.reviewRepository.AllAsNoTracking()
                .Where(r => r.PropertyId == propertyId)
                .Include(r => r.Tenant)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<ReviewViewModel>>(reviews);
        }

        public async Task<IEnumerable<DashboardReviewViewModel>> GetReviewsDashboard(Guid userId, IEnumerable<string> roles)
        {
            var reviews =  new List<Data.Models.Entities.Review>();

            if (roles.Contains("Tenant"))
            {
                reviews = await reviewRepository.AllAsNoTracking()
                    .Where(r => r.TenantId == userId && !r.IsDeleted)
                    .ToListAsync();
            }
            else if (roles.Contains("Owner"))
            {
                reviews = await reviewRepository.AllAsNoTracking()
                    .Where(r =>
                        !r.IsDeleted &&
                        !r.Property.IsDeleted &&
                        r.Property.OwnerId == userId
                    )
                    .ToListAsync();
            }

            return this.mapper.Map<IEnumerable<DashboardReviewViewModel>>(reviews);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, Guid tenantId)
        {
            var review = await this.reviewRepository.All()
                .FirstAsync(r => r.Id == reviewId && r.TenantId == tenantId);

            this.reviewRepository.Delete(review);
            var result = await this.reviewRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
