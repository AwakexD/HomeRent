using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class StatisticsController : AdministrationBaseController
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await this.statisticsService.GetAdminStatisticsAsync();

            return this.View(viewModel);
        }
    }
}
