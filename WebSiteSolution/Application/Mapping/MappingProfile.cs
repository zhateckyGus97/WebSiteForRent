using Application.Requests.ApartmentRequests;
using Application.Requests.DealRequests;
using Application.Requests.ReviewRequests;
using Application.Requests.UserRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationUserRequest, User>();
            CreateMap<UpdateUserRequest, User>();
            CreateMap<User, UserResponse>();

            CreateMap<CreateReviewRequest, Review>();
            CreateMap<UpdateReviewRequest, Review>();
            CreateMap<Review, ReviewResponse>();

            CreateMap<CreateDealRequest, Deal>();
            CreateMap<UpdateDealRequest, Deal>();
            CreateMap<Deal, DealResponse>();

            CreateMap<CreateApartmentRequest, Apartment>();
            CreateMap<UpdateApartmentRequest, Apartment>();
            CreateMap<Apartment, ApartmentResponse>();
        }
    }
}
