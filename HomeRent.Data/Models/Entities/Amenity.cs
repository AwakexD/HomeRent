using System.ComponentModel.DataAnnotations;
using HomeRent.Data.Common.Models;

namespace HomeRent.Data.Models.Entities
{
    public class Amenity : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IconClass { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}
