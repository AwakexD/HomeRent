namespace HomeRent.Models.ViewModels.Booking
{
    public class BookingOverviewViewModel : BookingBase
    {
        public string PropertyTitle { get; set; }

        public string PropertyAddress { get; set; }

        public string OwnerPhone { get; set; }

        public string OwnerEmail { get; set; }
    }
}
