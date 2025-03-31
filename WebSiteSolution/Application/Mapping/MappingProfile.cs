using Application.DTO;
using Application.Requests.ApartmentRequests;
using Application.Requests.DealRequests;
using Application.Requests.ReviewRequests;
using Application.Requests.UserReauests;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CreateUserRequest>().ReverseMap();
            CreateMap<Apartment, CreateApartmentRequest>().ReverseMap();
            CreateMap<Deal, CreateDealRequest>().ReverseMap();
            CreateMap<Review, CreateReviewRequest>().ReverseMap();
        }
    }
}
