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
<<<<<<< HEAD
using Application.Exceptions;
using Application.Requests.UserRequests;
=======
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
>>>>>>> a8c63b247bcbec1422d763623dbc6929dd1f3d28

namespace ApplicationUnitTests.Services
{
    public class UserServiceTests
    {
        private IUserRepository _userRepository;
<<<<<<< HEAD
        private IApartmentRepository _apartmentRepository;
        private IDealRepository _dealRepository;
        private IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private Faker _faker;

        public UserServiceTests()
        {
            _faker = new Faker();

            _userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = _userRepositoryMock.Object;
=======
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly User _user;
        private readonly ILogger<UserService> _logger;
        public UserServiceTests()
        {
            var faker = new Faker();

            _user = new User()
            {
                Id = 1,
                FullName = faker.Name.FullName(),
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = faker.Person.DateOfBirth
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(_user);
            _userRepository = userRepositoryMock.Object;
>>>>>>> a8c63b247bcbec1422d763623dbc6929dd1f3d28

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

<<<<<<< HEAD
            _userService = new UserService(_userRepository, _apartmentRepository, _dealRepository, _reviewRepository, _mapper);
=======
            _userService = new UserService(_userRepository, _mapper, _logger);
>>>>>>> a8c63b247bcbec1422d763623dbc6929dd1f3d28
        }

        [Fact] //(Skip = "Integration is not working now") можно скипнуть тест
        public void ShouldBeAvailableToCreate()
        {
            // Assert
            _userService.Should().NotBeNull();
        }

        [Fact]
<<<<<<< HEAD
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
=======
        public async void GetById_ShouldReturnUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(_user);
            _userRepository = userRepositoryMock.Object;

            var userService = new UserService(_userRepository, _mapper, _logger);
            int userId = 1;

            // Act
            var userDto = await userService.GetById(userId);

            // Assert
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be(_user.Id);
            userDto.FullName.Should().Be(_user.FullName);
            userDto.Passport.Should().Be(_user.Passport);
            userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetById_WhenRandomId_ShouldReturnUser(int id)
        {
            // Arrange
            var faker = new Faker();

            var user = new User()
            {
                Id = id,
                FullName = faker.Name.FullName(),
                Email = faker.Person.Email,
                PhoneNumber = faker.Person.Phone,
                Role = "test",
                Passport = "000000000",
                DateOfBirth = faker.Person.DateOfBirth
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
            _userRepository = userRepositoryMock.Object;

            var userService = new UserService(_userRepository, _mapper, _logger);

            // Act
            var userDto = await userService.GetById(id);

            // Assert
            userDto.Should().NotBeNull();
            userDto.Id.Should().Be(user.Id);
            userDto.FullName.Should().Be(_user.FullName);
            userDto.Passport.Should().Be(_user.Passport);
            userRepositoryMock.Verify(x => x.GetById(user.Id), Times.Once);
        }

        [Fact]
        public async Task GetAll_WhenNoUsers_ThrowsNotFoundApplicatonException()
        {
            //Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(new List<User>());
            _userRepository = userRepositoryMock.Object;
            var userService = new UserService(_userRepository, _mapper, _logger);

            // Act * Assert
            await FluentActions.Invoking(userService.GetAll)
                .Should()
                .ThrowAsync<DirectoryNotFoundException>() //not directory (application)
                .WithMessage("Users not found");
        }
    }
}
>>>>>>> a8c63b247bcbec1422d763623dbc6929dd1f3d28
