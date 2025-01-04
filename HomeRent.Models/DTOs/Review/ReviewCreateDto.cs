using System.ComponentModel.DataAnnotations;

namespace HomeRent.Models.DTOs.Review
{
    public class ReviewCreateDto
    {
        public string? Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime DateReviewed { get; set; } = DateTime.Now;
        
        public string? PropertyId { get; set; }
    }
}
