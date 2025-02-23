using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class AdminDashboardController : AdministrationBaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
