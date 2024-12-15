using System.ComponentModel.DataAnnotations;

namespace HomeRent.Models.Shared
{
    public class PropertyQueryModel
    {
        public string? Keyword { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public int? PropertyTypeId { get; set; }

        [Range(0, 100)]
        public int? MinBedrooms { get; set; }

        [Range(0, 100)]
        public int? MinBathrooms { get; set; }

        public int MinSize { get; set; }

        public int MaxSize { get; set; }

        public int Page { get; set; } = 1;

        public int ItemsPerPage { get; set; } = 3;

        public IEnumerable<int> AmenityIds { get; set; } = Enumerable.Empty<int>();
    }
}
