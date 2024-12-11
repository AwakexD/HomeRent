using HomeRent.Models.DTOs.Property;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyListItemViewModel>> GetListingsAsync();

        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);
    }
}
