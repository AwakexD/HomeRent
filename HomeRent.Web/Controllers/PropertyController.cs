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
    }
}
