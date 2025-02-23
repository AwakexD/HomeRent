using HomeRent.Models.Administration;

namespace HomeRent.Services.Administration.Contracts
{
    public interface IDataManagementService
    {
        Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync();

        Task<PropertyTypeAdminModel> GetPropertyTypeByIdAsync(int id);

        Task AddPropertyTypeAsync(PropertyTypeAdminModel model);

        Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model);

        Task DeletePropertyTypeAsync(int id, bool hardDelete);

        Task ActivatePropertyTypeAsync(int id);

        Task<IEnumerable<AmenityAdminModel>> GetAllAmenitiesAsync();

        Task<AmenityAdminModel> GetAmenityByIdAsync(int id);

        Task AddAmenityAsync(AmenityAdminModel model);

        Task UpdateAmenityAsync(int id, AmenityAdminModel model);

        Task ActivateAmenityAsync(int id);

        Task DeleteAmenityAsync(int id, bool hardDelete);
    }
}
