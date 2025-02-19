using HomeRent.Data.Enums;

namespace HomeRent.Models.ViewModels.Booking
{
    public class BookingTableViewModel : BookingBase
    {
        public string PropertyTitle { get; set; }

        public string PropertyImage { get; set; }

        public BookingStatus BookingStatus { get; set; }

        public bool IsDeleted { get; set; }

        public string TenantEmail { get; set; }

        public string TenantPhone { get; set; }

        public string PaymentMethod { get; set; }

        public string Message { get; set; }
    }
}
