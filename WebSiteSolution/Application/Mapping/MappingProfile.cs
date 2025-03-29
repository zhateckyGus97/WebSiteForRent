using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Apartment, ApartmentDTO>().ReverseMap();
            CreateMap<Deal, DealDTO>().ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }
}
