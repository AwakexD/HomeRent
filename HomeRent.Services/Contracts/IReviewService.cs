using HomeRent.Models.DTOs.Review;

namespace HomeRent.Services.Contracts
{
    public interface IReviewService
    {
        Task<bool> CreateReviewAsync(ReviewCreateDto reviewDto, Guid tenantId);

        Task<bool> DeleteReviewAsync(int reviewId, string username);
    }
}
