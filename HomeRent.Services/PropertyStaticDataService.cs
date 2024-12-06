using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.Shared;
using HomeRent.Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class PropertyStaticDataService : IPropertyStaticDataService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<Amenity> amenitiesRepository;

        public PropertyStaticDataService(IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<Amenity> amenitiesRepository)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.amenitiesRepository = amenitiesRepository;
        }

        public async Task<SelectList> GetPropertyTypesSelectList(int? selectedId = null)
        {
            var propertyTypes = await this.propertyTypesRepository.AllAsNoTracking()
                .Select(pt => new {pt.Id, pt.Name})
                .ToListAsync();

            return new SelectList(propertyTypes, "Id", "Name", selectedId);
        }

        public async Task<IEnumerable<AmenityViewModel>> GetAmenitiesSelectList(List<int> selectedAmenityIds = null)
        {
            var amenities = await this.amenitiesRepository.AllAsNoTracking()
                .Select(a => new AmenityViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    IconClass = a.IconClass,
                }).ToListAsync();

            return amenities;
        }
    }
}
