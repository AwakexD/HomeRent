using HomeRent.Models.DTOs.Property;

namespace HomeRent.Services.Contracts
{
    public interface IPropertyService
    {
        Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto);
    }
}
