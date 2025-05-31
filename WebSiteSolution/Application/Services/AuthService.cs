using Application.Requests.UserRequests;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Infrastructure.Interfaces;
using Application.Requests;
using AutoMapper;
using Application.Responses;

namespace Application.Services
{
    public class AuthService(IConfiguration configuration, IMapper mapper,
        IUserRepository userRepository, IPasswordHasher hasher) : IAuthService
    {
        public async Task<ClaimsPrincipal> Register(RegistrationUserRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);
            user.Role = UserRoles.User;

            var userId = await userRepository.Create(user);
            var createdUser = await userRepository.GetById(userId);

            var principal = GenerateClaimsPrincipal(createdUser!);

            return principal;
        }

        public async Task<ClaimsPrincipal?> Login(LoginRequest request)
        {
            var user = await userRepository.GetByEmail(request.Email);
            var passwordVerified = hasher.VerifyPassword(request.Password, user?.PasswordHash);
            if ((user == null || !passwordVerified))
            {
                throw new UnauthorizedAccessException();
            }

            var principal = GenerateClaimsPrincipal(user);

            return principal;
        }

        private ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {
            var identity = new ClaimsIdentity(new[]
            {
                 new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString() ?? UserRoles.User.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.FullName ?? string.Empty)
            }, "HttponlyAuth");

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
    }
}
