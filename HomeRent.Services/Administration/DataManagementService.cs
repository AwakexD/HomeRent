using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Administration;
using HomeRent.Services.Administration.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services.Administration
{
    public class DataManagementService : IDataManagementService
    {
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<Amenity> amenitiesRepository;
        private readonly IDeletableEntityRepository<Property> propertiesRepository;

        public DataManagementService(
            IMapper mapper,
            IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<Amenity> amenitiesRepository,
            IDeletableEntityRepository<Property> propertiesRepository)
        {
            this.mapper = mapper;
            this.propertyTypesRepository = propertyTypesRepository;
            this.amenitiesRepository = amenitiesRepository;
            this.propertiesRepository = propertiesRepository;
        }

        public async Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await propertyTypesRepository
                .AllAsNoTrackingWithDeleted()
                .ToListAsync();

            return this.mapper.Map<IEnumerable<PropertyTypeAdminModel>>(propertyTypes);
        }

        public async Task<PropertyTypeAdminModel> GetPropertyTypeByIdAsync(int id)
        {
            var propertyTypeEntity = await propertyTypesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (propertyTypeEntity == null)
            {
                throw new InvalidOperationException("PropertyTypes repository is empty.");
            }

            return this.mapper.Map<PropertyTypeAdminModel>(propertyTypeEntity);
        }

        public async Task AddPropertyTypeAsync(PropertyTypeAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Parameter cannot be null.");
            }

            try
            {
                // Map the admin model to the entity.
                var propertyType = this.mapper.Map<PropertyType>(model);
                await propertyTypesRepository.AddAsync(propertyType);
                await propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not add the property type.");
            }
        }

        public async Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Parameter cannot be null.");
            }

            try
            {
                var propertyType = await GetPropertyTypeByIdInternalAsync(id);

                this.mapper.Map(model, propertyType);

                propertyTypesRepository.Update(propertyType);
                await propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not update the property type.");
            }
        }

        public async Task DeletePropertyTypeAsync(int id, bool hardDelete)
        {
            var propertyType = await GetPropertyTypeByIdInternalAsync(id);

            if (hardDelete)
            {
                propertyTypesRepository.HardDelete(propertyType);
            }
            else
            {
                propertyTypesRepository.Delete(propertyType);
            }

            await propertyTypesRepository.SaveChangesAsync();
        }

        public async Task<bool> HasPropertiesOfTypeAsync(int propertyTypeId)
        {
            return await this.propertiesRepository
                .AllAsNoTracking()
                .AnyAsync(p => p.PropertyTypeId == propertyTypeId);
        }

        public async Task ActivatePropertyTypeAsync(int id)
        {
            try
            {
                var propertyType = await GetPropertyTypeByIdInternalAsync(id);
                propertyTypesRepository.Undelete(propertyType);
                await propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not activate the property type.");
            }
        }

        public async Task<IEnumerable<AmenityAdminModel>> GetAllAmenitiesAsync()
        {
            var amenities = await amenitiesRepository
                .AllAsNoTrackingWithDeleted()
                .ToListAsync();

            return this.mapper.Map<IEnumerable<AmenityAdminModel>>(amenities);
        }

        public async Task<AmenityAdminModel> GetAmenityByIdAsync(int id)
        {
            var amenityEntity = await amenitiesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (amenityEntity == null)
            {
                throw new InvalidOperationException("Feature not found.");
            }

            return this.mapper.Map<AmenityAdminModel>(amenityEntity);
        }

        public async Task AddAmenityAsync(AmenityAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Parameter cannot be null.");
            }

            try
            {
                // Map the admin model to the Amenity entity.
                var amenity = this.mapper.Map<Amenity>(model);
                await amenitiesRepository.AddAsync(amenity);
                await amenitiesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not add the feature.");
            }
        }

        public async Task UpdateAmenityAsync(int id, AmenityAdminModel model)
        {
            try
            {
                var amenity = await GetAmenityByIdInternalAsync(id);

                this.mapper.Map(model, amenity);

                amenitiesRepository.Update(amenity);
                await amenitiesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not update the feature.");
            }
        }

        public async Task DeleteAmenityAsync(int id, bool hardDelete)
        {
            var amenity = await GetAmenityByIdInternalAsync(id);

            if (hardDelete)
            {
                amenitiesRepository.HardDelete(amenity);
            }
            else
            {
                amenitiesRepository.Delete(amenity);
            }

            await amenitiesRepository.SaveChangesAsync();
        }

        public async Task<bool> HasRelatedPropertiesAsync(int amenityId)
        {
            return await this.amenitiesRepository
                .AllAsNoTracking()
                .AnyAsync(a => a.Id == amenityId && a.Properties.Any());
        }

        public async Task ActivateAmenityAsync(int id)
        {
            try
            {
                var amenity = await GetAmenityByIdInternalAsync(id);
                amenity.IsDeleted = false;
                await amenitiesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Could not activate the feature.");
            }
        }

        private async Task<Amenity> GetAmenityByIdInternalAsync(int id)
        {
            var amenity = await amenitiesRepository.AllWithDeleted()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (amenity == null)
            {
                throw new ArgumentNullException($"Feature with ID {id} not found.");
            }
            return amenity;
        }

        private async Task<PropertyType> GetPropertyTypeByIdInternalAsync(int id)
        {
            var propertyType = await propertyTypesRepository.AllWithDeleted()
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (propertyType == null)
            {
                throw new ArgumentNullException($"Property Type with ID {id} not found.");
            }
            return propertyType;
        }
    }
}
