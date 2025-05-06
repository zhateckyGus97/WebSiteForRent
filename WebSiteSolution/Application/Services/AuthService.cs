using Application.Requests.UserRequests;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<int> Register(RegistrationUserRequest request)
        {
            var user = mapper.Map<User>(request);
            user.PasswordHash = hasher.HashPassword(request.Password);
            user.Role = UserRoles.User;

            var userId = await userRepository.Create(user);

            return userId;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await userRepository.GetByEmail(request.Email);
            var passwordVerified = hasher.VerifyPassword(request.Password, user.PasswordHash);
            if((user == null || !passwordVerified))
            {
                throw new UnauthorizedAccessException();
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse() { Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSecret = configuration["JwtSettings:Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
            var jwtIssuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
            var jwtAudience = configuration["JwtSettings:Audience"] ??
                              throw new ArgumentNullException("JwtSettings:Audience");
            var jwtExpirationMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString() ?? UserRoles.User.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpirationMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
