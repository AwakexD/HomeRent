using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HomeRent.Models.ViewModels;
using HomeRent.Models.ViewModels.Home;
using HomeRent.Services.Contracts;

namespace HomeRent.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPropertyStaticDataService propertyStaticDataService;
        private readonly IPropertyService propertyService;

        public HomeController(IPropertyStaticDataService propertyStaticDataService,
            IPropertyService propertyService)
        {
            this.propertyStaticDataService = propertyStaticDataService;
            this.propertyService = propertyService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new HomeIndexViewModel()
                {
                    Properties = await this.propertyService.GetMostRecentListingsAsync(),
                    PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList(),
                    Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList()
                };
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
