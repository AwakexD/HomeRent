using AngleSharp.Common;
using HomeRent.Common;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;
using HomeRent.Models.ViewModels;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using HomeRent.Models.Administration;

namespace HomeRent.Web.Areas.Admin.Controllers
{
    public class PropertiesController : AdministrationBaseController
    {
        private readonly IPropertyService propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PropertyQueryModel query)
        {
            try
            {
                var (propertiesList, listingsCount) = await propertyService.GetListingsAdminDashboard(query);
                
                var viewModel = new PropertiesIndexViewModel()
                {
                    Properties = propertiesList,
                    Paging = new PagingViewModel()
                    {
                        PageNumber = query.Page,
                        ListingCount = listingsCount,
                        ItemsPerPage = query.ItemsPerPage,
                        QueryString = BuildQueryString(query)
                    }
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error",
                    new ErrorViewModel { ErrorMessage = ErrorConstants.PropertyListingsRetrieveError });

            }
        }

        private string BuildQueryString(PropertyQueryModel q)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(q.Keyword))
                parts.Add($"Keyword={Uri.EscapeDataString(q.Keyword)}");
            if (!string.IsNullOrWhiteSpace(q.Address))
                parts.Add($"Address={Uri.EscapeDataString(q.Address)}");
            if (!string.IsNullOrWhiteSpace(q.City))
                parts.Add($"City={Uri.EscapeDataString(q.City)}");
            if (q.PropertyTypeId.HasValue)
                parts.Add($"PropertyTypeId={q.PropertyTypeId.Value}");
            if (q.MinPrice.HasValue)
                parts.Add($"MinPrice={q.MinPrice.Value}");
            if (q.MaxPrice.HasValue)
                parts.Add($"MaxPrice={q.MaxPrice.Value}");
            if (q.MinSize.HasValue)
                parts.Add($"MinSize={q.MinSize.Value}");
            if (q.MaxSize.HasValue)
                parts.Add($"MaxSize={q.MaxSize.Value}");
            if (q.MinBedrooms.HasValue)
                parts.Add($"MinBedrooms={q.MinBedrooms.Value}");
            if (q.MinBathrooms.HasValue)
                parts.Add($"MinBathrooms={q.MinBathrooms.Value}");

            // multi‐value amenities
            foreach (var id in q.AmenityIds ?? Enumerable.Empty<int>())
                parts.Add($"AmenityIds={id}");

            return string.Join("&", parts);
        }
    }
}
