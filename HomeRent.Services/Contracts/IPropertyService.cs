using HomeRent.Data.Models.Entities;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAsync(PropertyQueryModel query);

        Task<IEnumerable<PropertyListItemViewModel>> GetMostRecentListingsAsync();

        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);

        Task<CreatePropertyDto> GetPropertyEditDataAsync(Guid propertyId, Guid userId);

        Task UpdatePropertyAsync(Guid propertyId, Guid userId, CreatePropertyDto updatedPropertyDto);

        Task<SinglePropertyViewModel> GetPropertyDetails(Guid propertyId);

        Task<bool> DeletePropertyImageAsync(Guid propertyId, string publicId);

        Task<int> GetTotalListingsCountAsync();
    }
}
