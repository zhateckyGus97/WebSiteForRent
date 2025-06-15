using Application.Interfaces;
using Application.Mapping;
using Application.Requests;
using Application.Requests.UserRequests;
using Application.Services;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IAuthService _authService;
        private Mock<IConfiguration> _configurationMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordHasher> _passwordHasherMock;
        private Faker _faker;

        public AuthServiceTests()
        {
            _faker = new Faker();

            _configurationMock = new Mock<IConfiguration>();
            _configuration = _configurationMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;

            _passwordHasherMock = new Mock<IPasswordHasher>();
            _passwordHasher = _passwordHasherMock.Object;

            SetupConfiguration();

            _authService = new AuthService(_configuration, _mapper, _userRepository, _passwordHasher);
        }

        private void SetupConfiguration()
        {
            var configSectionMock = new Mock<IConfigurationSection>();
            configSectionMock.Setup(x => x.Value).Returns("TestSecretKey12345678901234567890");
            _configurationMock.Setup(x => x.GetSection("JwtSettings:Secret")).Returns(configSectionMock.Object);

            configSectionMock = new Mock<IConfigurationSection>();
            configSectionMock.Setup(x => x.Value).Returns("TestIssuer");
            _configurationMock.Setup(x => x.GetSection("JwtSettings:Issuer")).Returns(configSectionMock.Object);

            configSectionMock = new Mock<IConfigurationSection>();
            configSectionMock.Setup(x => x.Value).Returns("TestAudience");
            _configurationMock.Setup(x => x.GetSection("JwtSettings:Audience")).Returns(configSectionMock.Object);

            configSectionMock = new Mock<IConfigurationSection>();
            configSectionMock.Setup(x => x.Value).Returns("60");
            _configurationMock.Setup(x => x.GetSection("JwtSettings:ExpirationInMinutes")).Returns(configSectionMock.Object);
        }

        [Fact]
        public async Task Register_ValidRequest_ReturnsUserId()
        {
            // Arrange
            var request = new RegistrationUserRequest
            {
                FullName = _faker.Name.FullName(),
                Email = _faker.Internet.Email(),
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Password = "Test123!",
                Passport = "123456789",
                DateOfBirth = _faker.Date.Past(20)
            };

            var userId = _faker.Random.Int(1, 100);
            var hashedPassword = "hashed_password";

            _passwordHasherMock.Setup(x => x.HashPassword(request.Password)).Returns(hashedPassword);
            _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(userId);

            // Act
            var result = await _authService.Register(request);

            // Assert
            result.Should().Be(userId);
            _userRepositoryMock.Verify(x => x.Create(It.Is<User>(u =>
                u.FullName == request.FullName &&
                u.Email == request.Email &&
                u.PhoneNumber == request.PhoneNumber &&
                u.Passport == request.Passport &&
                u.DateOfBirth == request.DateOfBirth &&
                u.PasswordHash == hashedPassword &&
                u.Role == UserRoles.User)), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var request = new LoginRequest
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            _userRepositoryMock.Setup(x => x.GetByEmail(request.Email)).ReturnsAsync((User)null);

            // Act & Assert
            await _authService.Invoking(x => x.Login(request))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

    }
}
