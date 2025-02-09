namespace HomeRent.Models.DTOs.Property
{
    public class DeleteImageRequest
    {
        public Guid PropertyId { get; set; }

        public string PublicId { get; set; }
    }
}
