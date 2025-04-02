using Application.DTO;
using Application.Requests.ApartmentRequests;
using Application.Requests.DealRequests;
using Application.Requests.ReviewRequests;
using Application.Requests.UserReauests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequest, User>().ReverseMap();
            CreateMap<UpdateUserRequest, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();

            CreateMap<CreateReviewRequest, Review>().ReverseMap();
            CreateMap<UpdateReviewRequest, Review>().ReverseMap();
            CreateMap<ReviewResponse, Review>().ReverseMap();

            CreateMap<CreateDealRequest, Deal>().ReverseMap();
            CreateMap<UpdateDealRequest, Deal>().ReverseMap();
            CreateMap<DealResponse, Deal>().ReverseMap();

            CreateMap<CreateApartmentRequest, Apartment>().ReverseMap();
            CreateMap<UpdateApartmentRequest, Apartment>().ReverseMap();
            CreateMap<ApartmentResponse, Apartment>().ReverseMap();
        }
    }
}
