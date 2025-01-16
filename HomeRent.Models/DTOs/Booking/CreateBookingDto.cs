namespace HomeRent.Models.DTOs.Booking
{
    public class CreateBookingDto
    {
        public Guid PropertyId { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }
    }
}
