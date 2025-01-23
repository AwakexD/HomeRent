namespace HomeRent.Models.ViewModels.Booking
{
    public class BookingOverviewViewModel
    {
        public Guid Id { get; set; }

        public string PropertyTitle { get; set; }

        public string PropertyAddress { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public string OwnerPhone { get; set; }

        public string OwnerEmail { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
