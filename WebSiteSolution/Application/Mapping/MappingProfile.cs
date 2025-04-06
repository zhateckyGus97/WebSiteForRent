using Application.Requests.ApartmentRequests;
using Application.Requests.DealRequests;
using Application.Requests.ReviewRequests;
using Application.Requests.UserRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<UserResponse, User>();

            CreateMap<CreateReviewRequest, Review>();
            CreateMap<UpdateReviewRequest, Review>();
            CreateMap<ReviewResponse, Review>();

            CreateMap<CreateDealRequest, Deal>();
            CreateMap<UpdateDealRequest, Deal>();
            CreateMap<DealResponse, Deal>();

            CreateMap<CreateApartmentRequest, Apartment>();
            CreateMap<UpdateApartmentRequest, Apartment>();
            CreateMap<ApartmentResponse, Apartment>();
        }
    }
}
