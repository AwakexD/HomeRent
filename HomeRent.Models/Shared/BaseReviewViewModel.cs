namespace HomeRent.Models.Shared
{
    public class BaseReviewViewModel
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public DateTime DateReviewed { get; set; }
    }
}