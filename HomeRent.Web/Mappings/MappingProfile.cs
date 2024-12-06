using AutoMapper;
using HomeRent.Data.Models.Entities;
using HomeRent.Models.Shared;

namespace HomeRent.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Amenity, AmenityViewModel>();
        }
    }
}
