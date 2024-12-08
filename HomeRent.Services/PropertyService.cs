using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Property;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Amenity> amenityRepository;
        private readonly IDeletableEntityRepository<PropertyImage> propertyImagesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public PropertyService(IMapper mapper,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Amenity> amenityRepository,
            IDeletableEntityRepository<PropertyImage> propertyImagesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.mapper = mapper;
            this.propertyRepository = propertyRepository;
            this.amenityRepository = amenityRepository;
            this.propertyImagesRepository = propertyImagesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task CreatePropertyAsync(Guid creatorId ,CreatePropertyDto propertyDto)
        {
            var property = this.mapper.Map<Property>(propertyDto);

            foreach (var imageFile in propertyDto.UploadedImages)
            {
                var url = await this.cloudinaryService.UploadImageAsync(imageFile);

                property.Images.Add(new PropertyImage() { ImageUrl = url});
            }

            var selectedAmenities = await amenityRepository.AllAsNoTracking()
                .Where(a => propertyDto.AmenityIds.Contains(a.Id))
                .ToListAsync();

            property.Amenities = selectedAmenities;
            property.OwnerId = creatorId;

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }
    }
}
