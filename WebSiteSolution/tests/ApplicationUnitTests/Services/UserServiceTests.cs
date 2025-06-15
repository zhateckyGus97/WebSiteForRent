using Application.Exceptions;
using Application.Interfaces;
using Application.Mapping;
using Application.Requests.UserRequests;
using Application.Responses;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApplicationUnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IApartmentRepository> _apartmentRepositoryMock;
        private readonly Mock<IDealRepository> _dealRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<IAttachmentService> _attachmentServiceMock;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _apartmentRepositoryMock = new Mock<IApartmentRepository>();
            _dealRepositoryMock = new Mock<IDealRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _attachmentServiceMock = new Mock<IAttachmentService>();
            _passwordHasherMock = new Mock<IPasswordHasher>();
            _loggerMock = new Mock<ILogger<UserService>>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _userService = new UserService(
                _userRepositoryMock.Object,
                _apartmentRepositoryMock.Object,
                _dealRepositoryMock.Object,
                _reviewRepositoryMock.Object,
                _mapper,
                _attachmentServiceMock.Object,
                _passwordHasherMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Add_ValidUser_ReturnsUserId()
        {
            // Arrange
            var request = new RegistrationUserRequest
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Password = "Password123!",
                PhoneNumber = "+1234567890",
                Passport = "AB1234567",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var expectedId = 1;
            var hashedPassword = "hashed_password";

            _passwordHasherMock.Setup(x => x.HashPassword(request.Password))
                .Returns(hashedPassword);

            _userRepositoryMock.Setup(x => x.Create(It.Is<User>(u =>
                u.FullName == request.FullName &&
                u.Email == request.Email &&
                u.PhoneNumber == request.PhoneNumber &&
                u.Passport == request.Passport &&
                u.DateOfBirth == request.DateOfBirth &&
                u.PasswordHash == hashedPassword &&
                u.Role == UserRoles.User)))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _userService.Add(request);

            // Assert
            result.Should().Be(expectedId);
            _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("created")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetById_ExistingUser_ReturnsUserResponse()
        {
            // Arrange
            var userId = 1;
            var user = new User
            {
                Id = userId,
                FullName = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "+1234567890",
                Passport = "AB1234567",
                DateOfBirth = new DateTime(1990, 1, 1),
                Role = UserRoles.User
            };

            _userRepositoryMock.Setup(x => x.GetById(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.FullName.Should().Be(user.FullName);
            result.Email.Should().Be(user.Email);
        }

        [Fact]
        public async Task GetById_NonExistingUser_ThrowsNotFoundException()
        {
            // Arrange
            var userId = 999;
            _userRepositoryMock.Setup(x => x.GetById(userId))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundApplicationException>(() =>
                _userService.GetById(userId));
        }

        [Fact]
        public async Task Delete_ExistingUser_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId };

            _userRepositoryMock.Setup(x => x.GetById(userId))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.Delete(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.Delete(userId);

            // Assert
            result.Should().BeTrue();
            _apartmentRepositoryMock.Verify(x => x.DeleteByOwnerId(userId), Times.Once);
            _reviewRepositoryMock.Verify(x => x.DeleteByUserId(userId), Times.Once);
            _dealRepositoryMock.Verify(x => x.DeleteByUserId(userId), Times.Once);
        }
    }
}