namespace HomeRent.Models.Shared
{
    public class SinglePropertyViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PropertyType { get; set; }

        public decimal PricePerNight { get; set; }

        public int Size { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public string OwnerFullName { get; set; }

        public string OwnerPhone { get; set; }

        public string OwnerEmail { get; set; }

        public IEnumerable<string> Images { get; set; }

        public IEnumerable<AmenityViewModel> Amenities { get; set; }

        // ToDO : Add Reviews
    }
}
