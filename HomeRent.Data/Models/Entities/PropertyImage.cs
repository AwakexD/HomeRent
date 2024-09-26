using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeRent.Data.Common.Models;

namespace HomeRent.Data.Models.Entities
{
    public class PropertyImage : BaseModel<Guid>
    {

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public Property Property { get; set; }
    }
}
