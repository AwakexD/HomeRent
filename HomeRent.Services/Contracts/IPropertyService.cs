using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAsync(PropertyQueryModel query);

        Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAdminDashboard(
            PropertyQueryModel query);

        Task<IEnumerable<PropertyListItemViewModel>> GetMostRecentListingsAsync();

        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);

        Task<CreatePropertyDto> GetPropertyEditDataAsync(Guid propertyId, Guid userId, bool isAdmin);

        Task UpdatePropertyAsync(Guid propertyId, Guid userId, CreatePropertyDto updatedPropertyDto);

        Task<SinglePropertyViewModel> GetPropertyDetails(Guid propertyId);

        Task<bool> DeletePropertyImageAsync(Guid propertyId, string publicId);

        Task<bool> DeletePropertyAsync(Guid propertyId, Guid userId, bool isAdmin);

        Task<bool> ActivatePropertyAsync(Guid propertyId, Guid userId, bool isAdmin);

        Task<int> GetTotalListingsCountAsync();
    }
}
