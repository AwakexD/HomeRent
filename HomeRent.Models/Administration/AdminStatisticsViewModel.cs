namespace HomeRent.Models.Administration
{
    public class AdminStatisticsViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalActiveProperties { get; set; }

        public int TotalBookings { get; set; }

        public int TotalReviews { get; set; }

        public decimal TotalRevenue { get; set; }

        public decimal AverageReservationPrice { get; set; }
    }
}
