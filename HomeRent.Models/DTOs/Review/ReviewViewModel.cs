namespace HomeRent.Models.DTOs.Review
{
    public class ReviewViewModel
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public DateTime DateReviewed { get; set; }

        public string TenantFullName { get; set; }
    }
}
