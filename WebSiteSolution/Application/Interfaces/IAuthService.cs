using Application.Requests;
using Application.Requests.UserRequests;
using Application.Responses;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<int> Register(RegistrationUserRequest request);
    }
}
