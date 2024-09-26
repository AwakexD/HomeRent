using HomeRent.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeRent.Data.Models.Entities
{
    public class Payment : BaseDeletableModel<Guid>
    {
        [Required]
        public string StripeTransactionId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }
    }
}
