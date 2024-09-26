using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeRent.Data.Common.Models;
using HomeRent.Data.Models.User;

namespace HomeRent.Data.Models.Entities
{
    public class Property : BaseDeletableModel<Guid>
    {
        public Property()
        {
            this.Images = new HashSet<PropertyImage>();
            this.Amenities = new HashSet<Amenity>();
            this.Bookings = new HashSet<Booking>();
        }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [MaxLength(5)]
        public string PostalCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerNight { get; set; }

        [Required]
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        [Required]
        [ForeignKey(nameof(PropertyType))]
        public int PropertyTypeId { get; set; }

        public PropertyType PropertyType { get; set; }

        public virtual ICollection<PropertyImage> Images { get; set; }

        public virtual ICollection<Amenity> Amenities { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

    }
}
