using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Models.DTOs.Property;
using HomeRent.Models.Shared;

namespace HomeRent.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Amenity, AmenityViewModel>();

            CreateMap<CreatePropertyDto, Property>()
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore());
        }
    }
}
