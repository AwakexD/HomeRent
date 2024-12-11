using HomeRent.Models.DTOs.Property;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyListItemViewModel>> GetListingsAsync(int page, int itemsPerPage);

        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);

        Task<int> GetTotalListingsCountAsync();
    }
}
