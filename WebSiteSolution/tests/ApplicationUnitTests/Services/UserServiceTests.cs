using Application.Services;
using AutoMapper;
using Infrastructure.Interfaces;
using Moq;
using Application.Mapping;
using FluentAssertions;
using Application.Interfaces;
using Domain.Entities;
using Bogus;
using Bogus.Extensions.Sweden;
using Application.Exceptions;
using Application.Requests.UserRequests;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class UserServiceTests
    {
        private IUserRepository _userRepository;
        private IApartmentRepository _apartmentRepository;
        private IDealRepository _dealRepository;
        private IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private readonly ILogger<IUserService> _logger;
        private Faker _faker;

        public UserServiceTests()
        {
            _faker = new Faker();

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<IUserService>.Instance;

            _userService = new UserService(_userRepository, _apartmentRepository, _dealRepository, _reviewRepository, _mapper, _logger);
        }

        [Fact] //(Skip = "Integration is not working now") можно скипнуть тест
        public void ShouldBeAvailableToCreate()
        {
            // Assert
            _userService.Should().NotBeNull();
        }

        [Fact]
        public async void Add_ValidRequest_ReturnsId()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            var request = new CreateUserRequest
            {
                FullName = _faker.Name.FullName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = _faker.Person.DateOfBirth
            };

            _userRepositoryMock.Setup(x => x.Create(It.Is<User>(u =>
                    u.FullName == request.FullName &&
                    u.Email == request.Email &&
                    u.PhoneNumber == request.PhoneNumber &&
                    u.Role == request.Role &&
                    u.Passport == request.Passport &&
                    u.DateOfBirth == request.DateOfBirth)))
                .ReturnsAsync(userId);

            // Act
            var result = await _userService.Add(request);

            // Assert
            result.Should().Be(userId);
            _userRepositoryMock.Verify(x => x.Create(It.Is<User>(u =>
                    u.FullName == request.FullName &&
                    u.Email == request.Email &&
                    u.PhoneNumber == request.PhoneNumber &&
                    u.Role == request.Role &&
                    u.Passport == request.Passport &&
                    u.DateOfBirth == request.DateOfBirth)), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnUser()
        {
            // Arrange
            var user = new User()
            {
                Id = 1,
                FullName = _faker.Name.FullName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = _faker.Person.DateOfBirth
            };

            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await _userService.GetById(user.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(user.Id);
            result.FullName.Should().Be(user.FullName);
            result.Passport.Should().Be(user.Passport);
            result.Email.Should().Be(user.Email);
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async void GetById_NonExistingUser_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _faker.Random.Int(1, 100);
            _userRepositoryMock.Setup(x => x.GetById(userId))
                .ReturnsAsync((User)null);

            // Act + Assert
            await _userService.Invoking(x => x.GetById(userId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage($"User with id {userId} not found!");
        }

        [Fact]
        public async void GetAll_WithUsers_ReturnsUsers()
        {
            //Arrange
            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FullName = _faker.Name.FullName(),
                    Email = _faker.Person.Email,
                    PhoneNumber = _faker.Person.Phone,
                    Role = "test",
                    Passport = "000000000",
                    DateOfBirth = _faker.Person.DateOfBirth
                },
                new User()
                {
                    Id = 2,
                    FullName = _faker.Name.FullName(),
                    Email = _faker.Person.Email,
                    PhoneNumber = _faker.Person.Phone,
                    Role = "test",
                    Passport = "000000000",
                    DateOfBirth = _faker.Person.DateOfBirth
                },
                new User()
                {
                    Id = 3,
                    FullName = _faker.Name.FullName(),
                    Email = _faker.Person.Email,
                    PhoneNumber = _faker.Person.Phone,
                    Role = "test",
                    Passport = "000000000",
                    DateOfBirth = _faker.Person.DateOfBirth
                }
            };

            _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(users);

            //Act
            var result = await _userService.GetAll();

            //Assert
            result.Should().HaveCount(3);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(3);
        }

        [Fact]
        public async void Update_ValidRequest_UpdateUser()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Id = 1,
                FullName = _faker.Name.FullName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = _faker.Person.DateOfBirth
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            await _userService.Update(request);

            // Assert
            _userRepositoryMock.Verify(x => x.Update(It.Is<User>(u =>
                    u.Id == request.Id &&
                    u.FullName == request.FullName &&
                    u.Email == request.Email &&
                    u.PhoneNumber == request.PhoneNumber &&
                    u.Role == request.Role &&
                    u.Passport == request.Passport &&
                    u.DateOfBirth == request.DateOfBirth)), Times.Once);
        }

        [Fact]
        public async void Update_NonExisting_ThrowNotFoundApplicationException()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Id = 1,
                FullName = _faker.Name.FullName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = _faker.Person.DateOfBirth
            };

            _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act + Assert
            await _userService.Invoking(x => x.Update((request)))
                .Should().ThrowAsync<EntityUpdateException>()
                .WithMessage("User wasn't updated!");
        }

        [Fact]
        public async void Delete_NonExistingUser_ThrowNotFountApplicationException()
        {
            //Arrange
            var userId = _faker.Random.Int(1, 100);

            _userRepositoryMock.Setup(x => x.Delete(userId))
                .ReturnsAsync(true);

            // Act + Assert
            await _userService.Invoking(x => x.Delete(userId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage($"User with id {userId} not found!");
        }
    }
}