namespace HomeRent.Models.ViewModels.Booking
{
    public class BookingBase
    {
        public Guid Id { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public Guid PropertyId { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
