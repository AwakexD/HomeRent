using HomeRent.Models.ViewModels.Dashboard;

namespace HomeRent.Services.Contracts
{
    public interface IDashboardService
    {
        Task<OwnerDashboardViewModel> GetOwnerDashboardVM(Guid userId);

        Task<TenantDashboardViewModel> GetTenantDashboardVM(Guid userId);
    }
}
