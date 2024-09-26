using System.ComponentModel.DataAnnotations;
using HomeRent.Data.Infrastructure;
using HomeRent.Data.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace HomeRent.Data.Models.User
{
    public class ApplicationUser : IdentityUser<Guid>, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.Roles = new HashSet<IdentityUserRole<Guid>>();
            this.Claims = new HashSet<IdentityUserClaim<Guid>>();
            this.Logins = new HashSet<IdentityUserLogin<Guid>>();

            this.OwnedProperties = new HashSet<Property>();
            this.Bookings = new HashSet<Booking>();
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        // Navigation properties
        public virtual ICollection<Property> OwnedProperties { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        // Identity-related collections
        public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
