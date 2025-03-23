using System.Security.Cryptography.X509Certificates;
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
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public IActionResult Delete(string userId)
        {
            return this.View();
        }
    }
}
