using HomeRent.Models.Administration;
using HomeRent.Models.ViewModels;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class PropertyTypesController : AdministrationBaseController
    {
        private readonly IDataManagementService dataManagementService;

        public PropertyTypesController(IDataManagementService dataManagementService)
        {
            this.dataManagementService = dataManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var viewModel = await this.dataManagementService.GetAllPropertyTypesAsync();

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PropertyTypeAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.dataManagementService.AddPropertyTypeAsync(model);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var propertyType = await dataManagementService.GetPropertyTypeByIdAsync(id);
            if (propertyType == null)
            {
                return this.View("Error", new ErrorViewModel());
            }

            return this.View(propertyType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PropertyTypeAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.dataManagementService.UpdatePropertyTypeAsync(model.Id, model);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, bool hardDelete)
        {
            try
            {
                if (hardDelete
                    && await this.dataManagementService.HasPropertiesOfTypeAsync(id))
                {
                    TempData["ErrorMessage"] =
                        "Не може да изтриете окончателно този тип имот – има свързани имоти.";
                    return RedirectToAction("All", "PropertyTypes");
                }

                await this.dataManagementService.DeletePropertyTypeAsync(id, hardDelete);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await this.dataManagementService.ActivatePropertyTypeAsync(id);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
