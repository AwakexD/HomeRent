using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeRent.Data.Common.Models;
using HomeRent.Data.Models.User;

namespace HomeRent.Data.Models.Entities
{
    public class Review : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(380)]
        public string Comment { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime DateReviewed { get; set; }

        [Required]
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public Property Property { get; set; } = null;

        [Required]
        [ForeignKey(nameof(Tenant))]
        public Guid TenantId { get; set; }

        public ApplicationUser Tenant { get; set; } = null;
    }
}
