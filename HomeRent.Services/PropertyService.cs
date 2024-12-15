using AutoMapper;
using Ganss.Xss;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Repositories.Contracts;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;
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

        public async Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAsync(PropertyQueryModel query)
        {
            var queryable = this.propertyRepository.AllAsNoTracking()
                .Include(p => p.Owner)
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                queryable = queryable.Where(p => p.City.Contains(query.City));
            }

            if (!string.IsNullOrWhiteSpace(query.Address))
            {
                queryable = queryable.Where(p => p.Address.Contains(query.Address));
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                queryable = queryable.Where(p => p.Title.Contains(query.Keyword) || p.Description.Contains(query.Keyword));
            }

            if (query.PropertyTypeId.HasValue)
            {
                queryable = queryable.Where(p => p.PropertyTypeId == query.PropertyTypeId);
            }

            if (query.MinPrice.HasValue)
            {
                queryable = queryable.Where(p => p.PricePerNight >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                queryable = queryable.Where(p => p.PricePerNight <= query.MaxPrice.Value);
            }

            if (query.MinBedrooms.HasValue)
            {
                queryable = queryable.Where(p => p.Bedrooms >= query.MinBedrooms.Value);
            }

            if (query.MinBathrooms.HasValue)
            {
                queryable = queryable.Where(p => p.Bathrooms == query.MinBathrooms.Value);
            }

            if (query.AmenityIds?.Any() == true)
            {
                queryable = queryable.Where(p => query.AmenityIds.All(a => p.Amenities.Any(pa => pa.Id == a)));
            }

            var properties = await queryable
                .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage)
                .ToListAsync();

            var mappedProperties = mapper.Map<IEnumerable<PropertyListItemViewModel>>(properties);
            var listingsCount = await queryable.CountAsync();

            return (mappedProperties, listingsCount);
        }

        public async Task<IEnumerable<PropertyListItemViewModel>> GetMostRecentListingsAsync()
        {
            var properties = await this.propertyRepository.AllAsNoTracking()
                .OrderByDescending(p => p.CreatedOn)
                .Include(p => p.Images)
                .Take(6)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<PropertyListItemViewModel>>(properties);
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

        public async Task<int> GetTotalListingsCountAsync() => await this.propertyRepository.AllAsNoTracking().CountAsync();

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
