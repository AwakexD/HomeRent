using HomeRent.Data.Enums;

namespace HomeRent.Models.Administration
{
    public class BookingPaymentViewModel
    {
        public Guid Id { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public BookingStatus BookingStatus { get; set; }

        public string TenantEmail { get; set; }

        public string TenantPhoneNumber { get; set; }

        public string PaymentId { get; set; }

        public string? StripeTransactionId { get; set; }

        public decimal AmountPaid { get; set; }

        public PaymentType PaymentType { get; set; }

        public string PaymentStatus { get; set; }
    }
}
