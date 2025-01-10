using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Models.ViewModels.Dashboard
{
    public class OwnerDashboardViewModel
    {
        public int ListingsCount { get; set; }
        
        public int RentedProperties { get; set; }

        public double AverageReviewRating { get; set; }

        public IEnumerable<SinglePropertyViewModel> Properties { get; set; }
    }
}
