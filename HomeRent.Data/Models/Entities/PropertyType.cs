using System.ComponentModel.DataAnnotations;
using HomeRent.Data.Infrastructure;

namespace HomeRent.Data.Models.Entities
{
    public class PropertyType : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
