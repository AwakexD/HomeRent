using HomeRent.Models.ViewModels.Property;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IPropertyStaticDataService propertyStaticDataService;

        public PropertyController(IPropertyStaticDataService propertyStaticDataService)
        {
            this.propertyStaticDataService = propertyStaticDataService;
        }

        public IActionResult All()
        {
            return this.View();
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreatePropertyViewModel()
            {
                PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList(),
                Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList(),
            };

            return this.View(viewModel);
        }
    }
}
