using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeRent.Data.Enums;
using HomeRent.Data.Infrastructure;
using HomeRent.Data.Models.User;

namespace HomeRent.Data.Models.Entities
{
    public class Booking : BaseDeletableModel<Guid>
    {
        public Booking()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }    

        [Required]
        public BookingStatus BookingStatus { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [ForeignKey(nameof(Tenant))]
        public Guid TenantId { get; set; }

        public ApplicationUser Tenant { get; set; } = null;

        [Required]
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public Property Property { get; set; } = null;

        [ForeignKey(nameof(Payment))]
        public Guid? PaymentId { get; set; }

        public Payment? Payment { get; set; } = null;
    }
}
