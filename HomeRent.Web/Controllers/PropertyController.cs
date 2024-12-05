using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    public class PropertyController : Controller
    {
        public PropertyController()
        {
            
        }

        public IActionResult All()
        {
            return this.View();
        }

        [Authorize(Roles = "Owner")]
        public IActionResult Create()
        {
            return this.View();
        }
    }
}
