using System.Collections;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using HomeRent.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace HomeRent.Models.DTOs.Property
{
    public class CreatePropertyDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Заглавието трябва да бъде между {2} и {1} символа.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(2000, MinimumLength = 30, ErrorMessage = "Описанието трябва да бъде между {2} и {1} символа.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(10, 10000000, ErrorMessage = "Цената за нощувка трябва да бъде между {2} и {1}")]
        public decimal PricePerNight { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Името на града трябва да бъде между {2} и {1} символа.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Адреса трябва да бъде между {2} и {1} символа.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(5, ErrorMessage = "Пощенския код не може да бъде повече от {2} символа")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(0, 100)]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        [Range(0, 100)]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "Полето е задължително")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<int> AmenityIds { get; set; } = new List<int>();

        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" })]
        public IEnumerable<IFormFile>? UploadedImages { get; set; }

        public IEnumerable<string> UploadedImagesUrls { get; set; } = Enumerable.Empty<string>();
    }
}
