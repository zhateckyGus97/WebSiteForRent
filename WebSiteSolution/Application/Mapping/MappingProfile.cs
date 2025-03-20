using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    internal class MappingProfile : Profile
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
