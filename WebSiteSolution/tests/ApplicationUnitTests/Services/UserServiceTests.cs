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

namespace ApplicationUnitTests.Services
{
    public class UserServiceTests
    {
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly User _user;
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

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _userService = new UserService(_userRepository, _mapper);
        }

        [Fact] //(Skip = "Integration is not working now") можно скипнуть тест
        public void ShouldBeAvailableToCreate()
        {
            // Assert
            _userService.Should().NotBeNull();
        }

        [Fact]
        public async void GetById_ShouldReturnUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(_user);
            _userRepository = userRepositoryMock.Object;

            var userService = new UserService(_userRepository, _mapper);
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

            var userService = new UserService(_userRepository, _mapper);

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
            var userService = new UserService(_userRepository, _mapper);

            // Act * Assert
            await FluentActions.Invoking(userService.GetAll)
                .Should()
                .ThrowAsync<DirectoryNotFoundException>() //not directory (application)
                .WithMessage("Users not found");
        }
    }
}
