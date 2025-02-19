using System.ComponentModel.DataAnnotations;

namespace HomeRent.Data.Enums
{
    public enum PaymentType
    {
        [Display(Name = "Плащане в брой")]
        Cash,

        [Display(Name = "Плащане с карта")]
        ByCard,
    }
}
