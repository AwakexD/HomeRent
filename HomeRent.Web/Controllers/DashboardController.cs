using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
