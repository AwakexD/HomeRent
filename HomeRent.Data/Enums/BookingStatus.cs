using System.ComponentModel.DataAnnotations;

namespace HomeRent.Data.Enums
{
    public enum BookingStatus
    {
        [Display(Name = "Потвърдена")]
        Confirmed,

        [Display(Name = "Непотвърдена")]
        Unconfirmed,

        [Display(Name = "Отказана")]
        Cancelled,

        [Display(Name = "Отказана от наемодател")]
        OwnerCancelled
    }
}
