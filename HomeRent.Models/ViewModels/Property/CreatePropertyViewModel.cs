using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Models.ViewModels.Property
{
    public class CreatePropertyViewModel
    {
        public CreatePropertyDto Property { get; set; } = new CreatePropertyDto();

        public IEnumerable<SelectListItem>? PropertyTypes { get; set; } = null;

        public IEnumerable<AmenityViewModel>? Amenities { get; set; } = null;
    }
}
