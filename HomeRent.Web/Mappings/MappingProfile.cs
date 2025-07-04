﻿using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Data.Models.User;
using HomeRent.Models.Administration;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.DTOs.Review;
using HomeRent.Models.Shared;
using HomeRent.Models.ViewModels.Booking;
using HomeRent.Models.ViewModels.Dashboard;
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
                .ForMember(dest => dest.UploadedImagesData, opt =>
                    opt.MapFrom(p => p.Images.Select(i => new UploadedImage { Url = i.ImageUrl, PublicId = i.PublicId})));

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

            CreateMap<Booking, BookingTableViewModel>()
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(b => b.Property.Title))
                .ForMember(dest => dest.PropertyImage,
                    opt => opt.MapFrom(b => b.Property.Images.FirstOrDefault().ImageUrl))
                .ForMember(dest => dest.TenantEmail, opt => opt.MapFrom(b => b.Tenant.Email))
                .ForMember(dest => dest.TenantPhone, opt => opt.MapFrom(b => b.Tenant.PhoneNumber))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(b =>
                    b.Payment == null
                        ? "Очаква се потвърждение"
                        : b.Payment.StripeTransactionId == "CASH_PAYMENT"
                            ? "Плащане в брой"
                            : "Плащане с карта"));

            CreateMap<Review, DashboardReviewViewModel>()
                .ForMember(dest => dest.TenantEmail,
                    opt => opt.MapFrom(r => r.Tenant.Email))
                .ForMember(dest => dest.PropertyTitle, opt => opt.MapFrom(r => r.Property.Title));

            // Admin Related Mapping Profiles

            CreateMap<ApplicationUser, UserViewModel>();

            CreateMap<Amenity, AmenityAdminModel>();

            CreateMap<AmenityAdminModel, Amenity>();

            CreateMap<PropertyType, PropertyTypeAdminModel>();

            CreateMap<PropertyTypeAdminModel, PropertyType>();

            CreateMap<Booking, BookingPaymentViewModel>()
                .ForMember(dest => dest.AmountPaid, opt => opt.MapFrom(b => b.Payment.AmountPaid))
                .ForMember(dest => dest.StripeTransactionId, opt => opt.MapFrom(b => b.Payment.StripeTransactionId));
            
            CreateMap<ApplicationUser, UserDeleteViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Role, opt => opt.Ignore());
        }
    }
}
