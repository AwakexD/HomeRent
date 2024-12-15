using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using HomeRent.Models.ViewModels;
using HomeRent.Models.ViewModels.Home;
using HomeRent.Services.Contracts;

namespace HomeRent.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IPropertyStaticDataService propertyStaticDataService;
        private readonly IPropertyService propertyService;

        public HomeController(ILogger<HomeController> logger,
            IPropertyStaticDataService propertyStaticDataService,
            IPropertyService propertyService)
        {
            this.logger = logger;
            this.propertyStaticDataService = propertyStaticDataService;
            this.propertyService = propertyService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeIndexViewModel()
            {
                Properties = await this.propertyService.GetMostRecentListingsAsync(),
                PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList(),
                Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
