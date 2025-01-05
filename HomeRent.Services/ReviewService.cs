using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Review;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Review> reviewRepository;

        public ReviewService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Review> reviewRepository)
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

            var review = this.mapper.Map<Review>(reviewDto);
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
