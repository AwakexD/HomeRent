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
                        ItemsPerPage = 12,
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
    }
}
