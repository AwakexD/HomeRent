﻿using HomeRent.Data.Models.User;
using HomeRent.Models.ViewModels.Property;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPropertyService propertyService;
        private readonly IPropertyStaticDataService propertyStaticDataService;

        public PropertyController(UserManager<ApplicationUser> userManager, IPropertyService propertyService,
            IPropertyStaticDataService propertyStaticDataService)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create(CreatePropertyViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.propertyService.CreatePropertyAsync(user.Id, inputModel.Property);

            return this.RedirectToAction(nameof(All));
        }
    }
}
