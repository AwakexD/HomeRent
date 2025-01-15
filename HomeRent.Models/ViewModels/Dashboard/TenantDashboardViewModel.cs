namespace HomeRent.Models.ViewModels.Dashboard
{
    public class TenantDashboardViewModel
    {
        public int BookingsCount { get; set; }

        public int FavoritesCount { get; set; }
        
        public int ReviewsCount { get; set; }

        // ToDO : Display tenant bookings
        // IEnumerable<int> Bookings { get; set; }
    }
}
