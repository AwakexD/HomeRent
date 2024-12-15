using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Models.ViewModels.Home
{
    public class HomeIndexViewModel : PropertyQueryModel
    {
        public IEnumerable<PropertyListItemViewModel> Properties { get; set; }

        public IEnumerable<SelectListItem>? PropertyTypes { get; set; }

        public IEnumerable<AmenityViewModel>? Amenities { get; set; }
    }
}
