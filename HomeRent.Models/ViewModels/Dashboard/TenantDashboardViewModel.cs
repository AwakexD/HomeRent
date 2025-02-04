using HomeRent.Models.ViewModels.Booking;

namespace HomeRent.Models.ViewModels.Dashboard
{
    public class TenantDashboardViewModel
    {
        public int BookingsCount { get; set; }

        public int FavoritesCount { get; set; }
        
        public int ReviewsCount { get; set; }

        public IEnumerable<BookingTableViewModel> Bookings { get; set; }
    }
}
