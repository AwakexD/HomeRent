using System.ComponentModel.DataAnnotations;
using HomeRent.Data.Common.Models;

namespace HomeRent.Data.Models.Entities
{
    public class PropertyType : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
