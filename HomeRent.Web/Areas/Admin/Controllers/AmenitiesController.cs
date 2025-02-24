using HomeRent.Models.Administration;
using HomeRent.Models.ViewModels;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class AmenitiesController : AdministrationBaseController
    {
        private readonly IDataManagementService dataManagementService;

        public AmenitiesController(IDataManagementService dataManagementService)
        {
            this.dataManagementService = dataManagementService;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                var viewModel = await this.dataManagementService.GetAllAmenitiesAsync();

                if (viewModel == null || !viewModel.Any())
                {
                    return View("Error", new ErrorViewModel());
                }

                return this.View(viewModel);
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AmenityAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.dataManagementService.AddAmenityAsync(model);

                return this.RedirectToAction("All", "Amenities");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var feature = await dataManagementService.GetAmenityByIdAsync(id);

            if (feature == null)
            {
                return this.View("Error", new ErrorViewModel());
            }

            return this.View(feature);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AmenityAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.dataManagementService.UpdateAmenityAsync(model.Id, model);

                return this.RedirectToAction("All", "Amenities");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, bool hardDelete)
        {
            try
            {
                await this.dataManagementService.DeleteAmenityAsync(id, hardDelete);

                return this.RedirectToAction("All", "Amenities");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await this.dataManagementService.ActivateAmenityAsync(id);

                return this.RedirectToAction("All", "Amenities");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
