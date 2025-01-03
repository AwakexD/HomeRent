using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Review;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper mapper;

        private readonly IDeletableEntityRepository<Property> propertyRepository;

        public ReviewService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyRepository)
        {
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
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

        public Task<bool> DeleteReviewAsync(int reviewId, string username)
        {
            throw new NotImplementedException();
        }
    }
}
