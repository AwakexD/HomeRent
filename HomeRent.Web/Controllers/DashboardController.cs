using HomeRent.Data.Models.User;
using HomeRent.Models.ViewModels.Dashboard;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IDashboardService dashbaordService;

        public DashboardController(UserManager<ApplicationUser> userManager,
            IDashboardService dashboardService)
        {
            this.userManager = userManager;
            this.dashbaordService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(User);

            var isTenant = currentUser != null && await userManager.IsInRoleAsync(currentUser, "Tenant");
            var isOwner = currentUser != null && await userManager.IsInRoleAsync(currentUser, "Owner");

            var viewModel = new DashboardViewModel
            {
                IsTenant = isTenant,
                IsOwner = isOwner,
            };

            if (isOwner)
            {
                viewModel.OwnerDashboard = await this.dashbaordService.GetOwenerDashboardVM(currentUser.Id);
            }

            return this.View(viewModel);
        }
    }
}
