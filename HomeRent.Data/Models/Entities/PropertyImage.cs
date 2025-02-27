﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeRent.Data.Infrastructure;

namespace HomeRent.Data.Models.Entities
{
    public class PropertyImage : BaseDeletableModel<Guid>
    {

        [Required]
        public string ImageUrl { get; set; }

        public string PublicId { get; set; }

        [Required]
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public Property Property { get; set; } = null;
    }
}
