namespace HomeRent.Models.ViewModels.Property
{
    public class PropertyListItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public decimal PricePerNight { get; set; }

        public int Size { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public string OwnerFullName { get; set; }

        public string ImageUrl { get; set; } 

        public string PropertyTypeName { get; set; }
    }
}
