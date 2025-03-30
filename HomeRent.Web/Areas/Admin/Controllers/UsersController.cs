using HomeRent.Models.ViewModels;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class UsersController : AdministrationBaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await this.userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return View("Error", new ErrorViewModel());
                }

                return View(users);
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var user = await this.userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var canBeDeleted = await this.userService.CanBeDeletedAsync(userId);

                if (!canBeDeleted)
                {
                    TempData["Error"] = "Потребителят не може да бъде изтрит, тъй като има свързани данни (имоти, резервации или ревюта).";
                    return RedirectToAction(nameof(Index));
                }

                return View(user);
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string userId)
        {
            try
            {
                var canBeDeleted = await this.userService.CanBeDeletedAsync(userId);

                if (!canBeDeleted)
                {
                    return View("Error", new ErrorViewModel());
                }

                await this.userService.DeleteUserAsync(userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }
    }
}
