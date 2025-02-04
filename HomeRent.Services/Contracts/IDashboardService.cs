using HomeRent.Models.ViewModels.Dashboard;

namespace HomeRent.Services.Contracts
{
    public interface IDashboardService
    {
        Task<OwnerDashboardViewModel> GetOwnerDashboardVm(Guid userId);

        Task<TenantDashboardViewModel> GetTenantDashboardVm(Guid userId);
    }
}
