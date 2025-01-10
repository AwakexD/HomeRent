using HomeRent.Models.ViewModels.Dashboard;
using System;
namespace HomeRent.Services.Contracts
{
    public interface IDashboardService
    {
        Task<OwnerDashboardViewModel> GetOwenerDashboardVM(Guid userId);   
    }
}
