using System.Security.Claims;
using HomeRent.Data.Models.User;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
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
        private readonly IReviewService reviewService;

        public PropertyController(UserManager<ApplicationUser> userManager, 
            IPropertyService propertyService,
            IPropertyStaticDataService propertyStaticDataService,
            IReviewService reviewService)
        {
            this.userManager = userManager;
            this.propertyService = propertyService;
            this.propertyStaticDataService = propertyStaticDataService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> All([FromQuery] PropertyQueryModel query)
        {
            var (propertiesList, listingsCount) = await this.propertyService.GetListingsAsync(query);

            var viewModel = new PropertyAllViewModel()
            {
                Properties = propertiesList,
                PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList(),
                Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList(),
                Paging = new PagingViewModel()
                {
                    PageNumber = query.Page,
                    ListingCount = listingsCount,
                    ItemsPerPage = query.ItemsPerPage,
                    QueryParameters = this.GenerateQueryParameters(query)
                }
            };
            return this.View(viewModel);
        }

        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePropertyViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList();
                inputModel.Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList();

                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(User);

            try
            {
                await this.propertyService.CreatePropertyAsync(user.Id, inputModel.Property);

                return this.RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = new PropertyDetailsViewModel()
            {
                Property = await this.propertyService.GetPropertyDetails(new Guid(id)),
                Reviews = await this.reviewService.GetPropertyReviewsAsync(new Guid(id)),
            };

            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await this.userManager.GetUserAsync(User);

            var viewModel = new CreatePropertyViewModel()
            {
                Property = await this.propertyService.GetPropertyEditDataAsync(new Guid(id), user.Id),
                PropertyTypes = await this.propertyStaticDataService.GetPropertyTypesSelectList(),
                Amenities = await this.propertyStaticDataService.GetAmenitiesSelectList()
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CreatePropertyDto updatedPropertyDto)
        {
            var user = await this.userManager.GetUserAsync(User);

            await this.propertyService.UpdatePropertyAsync(updatedPropertyDto.Id, user.Id, updatedPropertyDto);

            return this.RedirectToAction(nameof(All));
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
