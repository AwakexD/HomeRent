using AutoMapper;
using Ganss.Xss;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.ViewModels.Property;
using HomeRent.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly IHtmlSanitizer htmlSanitizer;
        private readonly ICloudinaryService cloudinaryService;

        private readonly IDeletableEntityRepository<Property> propertyRepository;
        private readonly IDeletableEntityRepository<Amenity> amenityRepository;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypeRepository;

        public PropertyService(IMapper mapper,
            IHtmlSanitizer htmlSanitizer,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Amenity> amenityRepository,
            IDeletableEntityRepository<PropertyType> propertyTypeRepository)
        {
            this.mapper = mapper;
            this.htmlSanitizer = htmlSanitizer;
            this.cloudinaryService = cloudinaryService;
            this.propertyRepository = propertyRepository;
            this.amenityRepository = amenityRepository;
            this.propertyTypeRepository = propertyTypeRepository;
        }

        public async Task<IEnumerable<PropertyListItemViewModel>> GetListingsAsync()
        {
            var listings = await this.propertyRepository.AllAsNoTracking()
                .Include(p => p.Owner)
                .Include(p => p.Images)
                .ToListAsync();

            return mapper.Map<IEnumerable<PropertyListItemViewModel>>(listings);
        }

        public async Task CreatePropertyAsync(Guid creatorId ,CreatePropertyDto propertyDto)
        {
            propertyDto = this.SanitizePropertyForm(propertyDto);

            var isValid = await this.ValidatePropertyForm(propertyDto);
            if (!isValid)
            {
                throw new ArgumentException("Invalid data in property creation form.");
            }

            var property = this.mapper.Map<Property>(propertyDto);


            var selectedAmenities = await amenityRepository.All()
                .Where(a => propertyDto.AmenityIds.Contains(a.Id))
                .ToListAsync();

            foreach (var imageFile in propertyDto.UploadedImages)
            {
                var url = await this.cloudinaryService.UploadImageAsync(imageFile);

                property.Images.Add(new PropertyImage() { ImageUrl = url});
            }

            property.Amenities = selectedAmenities;
            property.OwnerId = creatorId;

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        private async Task<bool> ValidatePropertyForm(CreatePropertyDto propertyDto)
        {
            var propertyTypeExists = await this.propertyTypeRepository.AllAsNoTracking().AnyAsync(pt => pt.Id == propertyDto.PropertyTypeId);

            var amenityIdsExist = !propertyDto.AmenityIds.Any() || await this.amenityRepository.AllAsNoTracking()
                    .Where(a => propertyDto.AmenityIds.Contains(a.Id))
                    .CountAsync() == propertyDto.AmenityIds.Count();

            return propertyTypeExists && amenityIdsExist;
        }

        private CreatePropertyDto SanitizePropertyForm(CreatePropertyDto propertyDto)
        {
            propertyDto.Title = this.htmlSanitizer.Sanitize(propertyDto.Title);
            propertyDto.Description = this.htmlSanitizer.Sanitize(propertyDto.Description);
            propertyDto.City = this.htmlSanitizer.Sanitize(propertyDto.City);
            propertyDto.Address = this.htmlSanitizer.Sanitize(propertyDto.Address);
            propertyDto.PostalCode = this.htmlSanitizer.Sanitize(propertyDto.PostalCode);

            return propertyDto;
        }

    }
}
