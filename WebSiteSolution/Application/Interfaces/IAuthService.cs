using Application.Requests;
using Application.Requests.UserRequests;
using Application.Responses;
using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> Login(LoginRequest request);
        Task<ClaimsPrincipal> Register(RegistrationUserRequest request);
    }
}
