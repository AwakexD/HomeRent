namespace HomeRent.Models.DTOs.Review
{
    public class ReviewCreateDto
    {
        public string? Comment { get; set; }

        public int Rating { get; set; }

        public DateTime DateReviewed { get; set; } = DateTime.Now;
        
        public string? PropertyId { get; set; }
    }
}
