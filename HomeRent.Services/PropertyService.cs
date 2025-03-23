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
        private readonly IDeletableEntityRepository<PropertyImage> propertyImageRepository;

        public PropertyService(IMapper mapper,
            IHtmlSanitizer htmlSanitizer,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Property> propertyRepository,
            IDeletableEntityRepository<Amenity> amenityRepository,
            IDeletableEntityRepository<PropertyType> propertyTypeRepository,
            IDeletableEntityRepository<PropertyImage> propertyImageRepository)
        {
            this.mapper = mapper;
            this.htmlSanitizer = htmlSanitizer;
            this.cloudinaryService = cloudinaryService;
            this.propertyRepository = propertyRepository;
            this.amenityRepository = amenityRepository;
            this.propertyTypeRepository = propertyTypeRepository;
            this.propertyImageRepository = propertyImageRepository;
        }

        public async Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAsync(PropertyQueryModel query)
        {
            var queryable = this.propertyRepository.AllAsNoTracking()
                .Include(p => p.Owner)
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .Include(p => p.PropertyType)
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

            if (query.MinSize.HasValue)
            {
                queryable = queryable.Where(p => p.Size >= query.MinSize.Value);
            }

            if (query.MaxSize.HasValue)
            {
                queryable = queryable.Where(p => p.Size <= query.MaxSize.Value);
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
                queryable = queryable.Where(p => p.Amenities.Any(q => query.AmenityIds.Contains(q.Id)));
            }

            var properties = await queryable
                .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage)
                .ToListAsync();

            var mappedProperties = mapper.Map<IEnumerable<PropertyListItemViewModel>>(properties);
            var listingsCount = await queryable.CountAsync();

            return (mappedProperties, listingsCount);
        }

        public async Task<(IEnumerable<PropertyListItemViewModel>, int listingsCount)> GetListingsAdminDashboard(PropertyQueryModel query)
        {
            var queryable = this.propertyRepository.AllAsNoTrackingWithDeleted()
                .Include(p => p.Owner)
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .Include(p => p.PropertyType)
                .AsQueryable();

            var listingsCount = await queryable.CountAsync();

            var properties = await queryable
            .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage)
                .ToListAsync();

            var mappedProperties = mapper.Map<IEnumerable<PropertyListItemViewModel>>(properties);

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

        public async Task CreatePropertyAsync(Guid creatorId, CreatePropertyDto propertyDto)
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


            var uploadResults = await Task.WhenAll(
                propertyDto.UploadedImages.Select(imageFile => this.cloudinaryService.UploadImageAsync(imageFile))
            );

            foreach (var result in uploadResults)
            {
               property.Images.Add(new PropertyImage()
               {
                   ImageUrl = result.secureUrl,
                   PublicId = result.publicId,
               });
            }

            property.Amenities = selectedAmenities;
            property.OwnerId = creatorId;

            await this.propertyRepository.AddAsync(property);
            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<CreatePropertyDto> GetPropertyEditDataAsync(Guid propertyId, Guid userId, bool isAdmin)
        {
            var property = await this.propertyRepository.AllAsNoTrackingWithDeleted()
                .Where(p => p.Id == propertyId && (isAdmin || p.OwnerId == userId))
                .Include(p => p.Amenities)
                .Include(p => p.PropertyType)
                .Include(p => p.Images)
                .SingleOrDefaultAsync();

            if (property == null)
            {
                throw new UnauthorizedAccessException("You do not have permission to edit this property.");
            }

            return this.mapper.Map<CreatePropertyDto>(property);
        }

        public async Task UpdatePropertyAsync(Guid propertyId, Guid userId, CreatePropertyDto updatedPropertyDto)
        {
            updatedPropertyDto = this.SanitizePropertyForm(updatedPropertyDto);

            var isValid = await this.ValidatePropertyForm(updatedPropertyDto);
            if (!isValid)
            {
                throw new ArgumentException("Invalid data in property edit form.");
            }

            var property = await this.propertyRepository.AllWithDeleted()
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .Include(p => p.PropertyType)
                .FirstOrDefaultAsync(p => p.Id == updatedPropertyDto.Id);

            if (property == null)
            {
                throw new InvalidOperationException("Property not found or access denied.");
            }

            this.mapper.Map(updatedPropertyDto, property);

            var selectedAmenities = await amenityRepository.All()
                .Where(a => updatedPropertyDto.AmenityIds.Contains(a.Id))
                .ToListAsync();

            property.Amenities.Clear();
            property.Amenities = selectedAmenities;

            await this.propertyRepository.SaveChangesAsync();
        }

        public async Task<SinglePropertyViewModel> GetPropertyDetails(Guid propertyId)
        {
            var property = await this.propertyRepository.AllAsNoTracking()
                .Include(p => p.Images)
                .Include(p => p.Owner)
                .Include(p => p.PropertyType)
                .Include(p => p.Amenities)
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            return this.mapper.Map<SinglePropertyViewModel>(property);
        }

        public async Task<bool> DeletePropertyImageAsync(Guid propertyId, string publicId)
        {
            if (propertyId == Guid.Empty || string.IsNullOrWhiteSpace(publicId))
            {
                throw new ArgumentException("Invalid parameters.");
            }

            var cloudinaryResult = await cloudinaryService.DeleteImageAsync(new List<string> { publicId });
            if (!cloudinaryResult)
            {
                throw new InvalidOperationException("Image deletion failed.");
            }

            var image = await this.propertyImageRepository.All()
                .FirstAsync(pi => pi.PropertyId == propertyId && pi.PublicId == publicId);

            this.propertyImageRepository.HardDelete(image);
            await this.propertyImageRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeletePropertyAsync(Guid propertyId, Guid userId, bool isAdmin)
        {
            var property = await GetAuthorizedPropertyAsync(propertyId, userId, isAdmin);
            if (property == null)
            {
                throw new InvalidOperationException($"Property deletion failed.");
            }

            this.propertyRepository.Delete(property);

            await this.propertyRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivatePropertyAsync(Guid propertyId, Guid userId, bool isAdmin)
        {
            var property = await GetAuthorizedPropertyAsync(propertyId, userId, isAdmin);

            if (property == null)
            {
                throw new InvalidOperationException("Property activation failed.");
            }

            this.propertyRepository.Undelete(property);
            await this.propertyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetTotalListingsCountAsync() => await this.propertyRepository.AllAsNoTracking().CountAsync();

        private async Task<Property> GetAuthorizedPropertyAsync(Guid propertyId, Guid userId, bool isAdmin)
        {
            var property = await this.propertyRepository.AllWithDeleted()
                .FirstOrDefaultAsync(p => p.Id == propertyId && (isAdmin || p.OwnerId == userId));

            return property;
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
