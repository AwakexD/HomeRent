using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAsync(PropertyQueryModel query);

        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);

        Task<int> GetTotalListingsCountAsync();
    }
}
