using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.DTOs.Review;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Booking;
using HomeRent.Models.ViewModels.Property;

namespace HomeRent.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Amenity, AmenityViewModel>();

            CreateMap<CreatePropertyDto, Property>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());

            CreateMap<Property, CreatePropertyDto>()
                .ForMember(dest => dest.AmenityIds, opt =>
                    opt.MapFrom(p => p.Amenities.Select(a => a.Id)))
                .ForMember(dest => dest.UploadedImagesUrls, opt =>
                    opt.MapFrom(p => p.Images.Select(i => i.ImageUrl)));

            CreateMap<Property, PropertyListItemViewModel>()
                .ForMember(dest => dest.Description, opt => 
                    opt.MapFrom(p => 
                        p.Description.Length > 150 ? p.Description.Substring(0, 150) + "..." : p.Description))
                .ForMember(dest => dest.OwnerFullName,
                    opt => 
                        opt.MapFrom(p => string.Join(" ", p.Owner.FirstName, p.Owner.LastName)))
                .ForMember(dest => dest.ImageUrl,
                    opt => 
                        opt.MapFrom(p => p.Images.OrderBy(i => Guid.NewGuid()).FirstOrDefault().ImageUrl));

            CreateMap<Property, SinglePropertyViewModel>()
                .ForMember(dest => dest.PropertyType,
                    opt => opt.MapFrom(p => p.PropertyType.Name))
                .ForMember(dest => dest.OwnerFullName,
                    opt =>
                        opt.MapFrom(p => string.Join(" ", p.Owner.FirstName, p.Owner.LastName)))
                .ForMember(dest => dest.OwnerPhone,
                    opt => opt.MapFrom(p => p.Owner.PhoneNumber))
                .ForMember(dest => dest.OwnerEmail,
                    opt => opt.MapFrom(p => p.Owner.Email))
                .ForMember(dest => dest.Images,
                    opt => opt.MapFrom(
                        p => p.Images.OrderBy(i => Guid.NewGuid()).Select(i => i.ImageUrl)))
                .ForMember(dest => dest.Amenities,
                    opt => opt.MapFrom(
                        p => p.Amenities.Select(a => new AmenityViewModel() { Id = a.Id, IconClass = a.IconClass, Name = a.Name})));

            CreateMap<ReviewCreateDto, Review>()
                .ForMember(dest => dest.TenantId, opt => opt.Ignore());

            CreateMap<Review, ReviewViewModel>()
                .ForMember(dest => dest.TenantFullName, opt =>
                    opt.MapFrom(r => string.Join(" ", r.Tenant.FirstName, r.Tenant.LastName)));

            CreateMap<Booking, BookingOverviewViewModel>()
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(b => b.Property.Title))
                .ForMember(dest => dest.PropertyAddress, opt => opt.MapFrom(b => b.Property.Address))
                .ForMember(dest => dest.OwnerPhone, opt => opt.MapFrom(b => b.Property.Owner.PhoneNumber))
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(b => b.Property.Owner.Email));
        }
    }
}
