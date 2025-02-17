using HomeRent.Models.DTOs.Review;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Dashboard;

namespace HomeRent.Services.Contracts
{
    public interface IReviewService
    {
        Task<bool> CreateReviewAsync(ReviewCreateDto reviewDto, Guid tenantId);

        Task<IEnumerable<ReviewViewModel>> GetPropertyReviewsAsync(Guid propertyId);

        Task<IEnumerable<DashboardReviewViewModel>> GetReviewsDashboard(Guid userId, IEnumerable<string> roles);

        Task<bool> DeleteReviewAsync(int reviewId, Guid tenantId);
    }
}
