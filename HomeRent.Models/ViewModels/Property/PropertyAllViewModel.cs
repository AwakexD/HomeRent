﻿using HomeRent.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeRent.Models.ViewModels.Property
{
    public class PropertyAllViewModel
    {
        public IEnumerable<PropertyListItemViewModel>? Properties { get; set; }

        public IEnumerable<SelectListItem>? PropertyTypes { get; set; }

        public IEnumerable<AmenityViewModel>? Amenities { get; set; }
    }
}