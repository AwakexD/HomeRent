using HomeRent.Models.DTOs.Review;
using HomeRent.Models.Shared;

namespace HomeRent.Models.ViewModels.Property
{
    public class PropertyDetailsViewModel
    {
        public SinglePropertyViewModel Property { get; set; }

        public IEnumerable<ReviewViewModel> Reviews { get; set; }
    }
}
