using HomeRent.Models.DTOs.Review;

namespace HomeRent.Services.Contracts
{
    public interface IReviewService
    {
        Task<bool> CreateReviewAsync(ReviewCreateDto reviewDto, Guid tenantId);

        Task<IEnumerable<ReviewViewModel>> GetPropertyReviewsAsync(Guid propertyId);

        Task<bool> DeleteReviewAsync(int reviewId, Guid tenantId);
    }
}
