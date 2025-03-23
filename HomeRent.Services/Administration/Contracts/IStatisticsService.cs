using HomeRent.Models.Administration;

namespace HomeRent.Services.Administration.Contracts
{
    public interface IStatisticsService
    {
        Task<AdminStatisticsViewModel> GetAdminStatisticsAsync();
    }
}
