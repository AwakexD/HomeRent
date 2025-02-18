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
        private readonly IReviewService reviewService;

        public DashboardController(UserManager<ApplicationUser> userManager,
            IDashboardService dashboardService,
            IReviewService reviewService)
        {
            this.userManager = userManager;
            this.dashboardService = dashboardService;
            this.reviewService = reviewService;
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

        public async Task<IActionResult> Reviews()
        {
            var user = await this.userManager.GetUserAsync(User);
            var roles = await this.userManager.GetRolesAsync(user);

            var reviews = await this.reviewService.GetReviewsDashboard(user.Id, roles);

            ViewBag.HideFooter = true;
            return this.View(reviews);
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Bookings()
        {
            // ToDO : Complete action : Create service method and retrieve bookings from the DB.

            ViewBag.HideFooter = true;
            return this.View();
        }
    }
}
