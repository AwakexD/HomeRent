using HomeRent.Data.Models.User;
using HomeRent.Models.ViewModels.Dashboard;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IDashboardService dashboardService;

        public DashboardController(UserManager<ApplicationUser> userManager,
            IDashboardService dashboardService)
        {
            this.userManager = userManager;
            this.dashboardService = dashboardService;
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
                viewModel.OwnerDashboard = await this.dashboardService.GetOwnerDashboardVm(currentUser.Id);
            }
            else if (isTenant)
            {
                viewModel.TenantDashboard = await this.dashboardService.GetTenantDashboardVm(currentUser.Id);
            }

            ViewBag.HideFooter = true;
            return this.View(viewModel);
        }
    }
}
