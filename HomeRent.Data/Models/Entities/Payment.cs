using HomeRent.Data.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeRent.Data.Models.Entities
{
    public class Payment : BaseDeletableModel<Guid>
    {
        public Payment()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        public string StripeTransactionId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AmountPaid { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        public DateTime PaymentDate { get; set; }

        [ForeignKey(nameof(Booking))]
        public Guid BookingId { get; set; }

        public Booking Booking { get; set; }
    }
}
