﻿using HomeRent.Common;
using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels;
using HomeRent.Models.ViewModels.Property;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Web.Controllers
{
    public class PropertyController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IPropertyService propertyService;
        private readonly IPropertyStaticDataService propertyStaticDataService;
        private readonly IReviewService reviewService;
        private readonly ICloudinaryService cloudinaryService;

        public PropertyController(UserManager<ApplicationUser> userManager, 
            IPropertyService propertyService,
            IPropertyStaticDataService propertyStaticDataService,
            IReviewService reviewService,
            ICloudinaryService cloudinaryService)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
            this.propertyStaticDataService = propertyStaticDataService;
            this.reviewService = reviewService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] PropertyQueryModel query)
        {
            try
            {
                var (propertiesList, listingsCount) = await propertyService.GetListingsAsync(query);

                var viewModel = new PropertyAllViewModel()
                {
                    Properties = propertiesList,
                    PropertyTypes = await propertyStaticDataService.GetPropertyTypesSelectList(),
                    Amenities = await propertyStaticDataService.GetAmenitiesSelectList(),
                    Paging = new PagingViewModel()
                    {
                        PageNumber = query.Page,
                        ListingCount = listingsCount,
                        ItemsPerPage = query.ItemsPerPage,
                        QueryParameters = GenerateQueryParameters(query)
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyListingsRetrieveError });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var viewModel = new CreatePropertyViewModel()
                {
                    PropertyTypes = await propertyStaticDataService.GetPropertyTypesSelectList(),
                    Amenities = await propertyStaticDataService.GetAmenitiesSelectList(),
                };

                ViewBag.HideFooter = true;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyCreateRetrieveError });
            }
        }
        
        [HttpPost]
        [Authorize(Roles = "Owner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePropertyViewModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                inputModel.PropertyTypes = await propertyStaticDataService.GetPropertyTypesSelectList();
                inputModel.Amenities = await propertyStaticDataService.GetAmenitiesSelectList();

                return View(inputModel);
            }

            var user = await userManager.GetUserAsync(User);

            try
            {
                await propertyService.CreatePropertyAsync(user.Id, inputModel.Property);

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyCreateError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var propertyId = new Guid(id);

                var viewModel = new PropertyDetailsViewModel()
                {
                    Property = await propertyService.GetPropertyDetails(propertyId),
                    Reviews = await reviewService.GetPropertyReviewsAsync(propertyId),
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyDetailsRetrieveError });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Owner,Administrator")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                var propertyId = new Guid(id);

                bool isAdmin = this.User.IsInRole("Administrator");

                var viewModel = new CreatePropertyViewModel()
                {
                    Property = await propertyService.GetPropertyEditDataAsync(propertyId, user.Id, isAdmin),
                    PropertyTypes = await propertyStaticDataService.GetPropertyTypesSelectList(),
                    Amenities = await propertyStaticDataService.GetAmenitiesSelectList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyEditRetrieveError });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Owner,Administrator")]
        public async Task<IActionResult> Edit(CreatePropertyDto updatedPropertyDto)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);

                await propertyService.UpdatePropertyAsync(updatedPropertyDto.Id, user.Id, updatedPropertyDto);

                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyEditUpdateError });
            } 
        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto([FromBody] DeleteImageRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.PublicId))
            {
                return Json(new { success = false, message = "Invalid image data." });
            }

            var result = await this.propertyService.DeletePropertyImageAsync(request.PropertyId, request.PublicId);
            if (result)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Failed to delete image." });
        }

        [HttpPost]
        [Authorize(Roles = "Owner,Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(Guid propertyId)
        {
            var user = await userManager.GetUserAsync(User);
            bool isAdmin = User.IsInRole("Administrator");

            try
            {
                await propertyService.DeletePropertyAsync(propertyId, user.Id, isAdmin);
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Property soft delete (deactivation) failed." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Owner,Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(Guid propertyId)
        {
            var user = await userManager.GetUserAsync(User);
            bool isAdmin = User.IsInRole("Administrator");

            try
            {
                await propertyService.ActivatePropertyAsync(propertyId, user.Id, isAdmin);
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { ErrorMessage = "Property activation (undelete) failed." });
            }
        }

        private Dictionary<string, string> GenerateQueryParameters(PropertyQueryModel queryModel)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(queryModel.Keyword))
                queryParams["Keyword"] = queryModel.Keyword;

            if (!string.IsNullOrEmpty(queryModel.Address))
                queryParams["Address"] = queryModel.Address;

            if (!string.IsNullOrEmpty(queryModel.City))
                queryParams["City"] = queryModel.City;

            if (queryModel.PropertyTypeId.HasValue)
                queryParams["PropertyTypeId"] = queryModel.PropertyTypeId.Value.ToString();

            if (queryModel.MinBathrooms.HasValue)
                queryParams["MinBathrooms"] = queryModel.MinBathrooms.Value.ToString();

            if (queryModel.MinBedrooms.HasValue)
                queryParams["MinBedrooms"] = queryModel.MinBedrooms.Value.ToString();

            if (queryModel.MinPrice.HasValue)
                queryParams["MinPrice"] = queryModel.MinPrice.Value.ToString();

            if (queryModel.MaxPrice.HasValue)
                queryParams["MaxPrice"] = queryModel.MaxPrice.Value.ToString();

            if (queryModel.AmenityIds?.Any() == true)
            {
                queryParams["SelectedAmenities"] = string.Join(",", queryModel.AmenityIds);
            }

            return queryParams;
        }
    }
}
