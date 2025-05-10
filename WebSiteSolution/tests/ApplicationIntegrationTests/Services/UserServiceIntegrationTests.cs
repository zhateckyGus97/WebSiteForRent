using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.UserRequests;
using ApplicationIntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

public class UserServiceTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture _fixture;
    private readonly IUserService _userService;

    public UserServiceTests(TestingFixture fixture)
    {
        _fixture = fixture;
        var scope = fixture.ServiceProvider.CreateScope();
        _userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task Add_ShouldCreateUser_AndReturnId()
    {
        //Arrange
        var request = new RegistrationUserRequest
        {
            FullName = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Passport = "AB1234567",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        //Act
        var userId = await _userService.Add(request);

        //Assert
        userId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        //Arrange
        var user = await _fixture.CreateUser();
        var result = await _userService.GetById(user.Id);

        //Act
        result.Should().NotBeNull();

        //Assert
        result.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetById_ShouldThrow_WhenUserNotExists()
    {
        //Act
        Func<Task> act = async () => await _userService.GetById(-1);

        //Assert
        await act.Should().ThrowAsync<NotFoundApplicationException>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        //Arrange
        await _fixture.CreateUser();
        await _fixture.CreateUser();

        //Act
        var result = await _userService.GetAll();

        //Assert
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyUser()
    {
        //Arrange
        var user = await _fixture.CreateUser();
        var request = new UpdateUserRequest
        {
            Id = user.Id,
            FullName = "Updated Name",
            Email = "updated@example.com",
            PhoneNumber = "0987654321",
            Passport = "XY9876543",
            DateOfBirth = new DateTime(1985, 5, 15)
        };

        //Act
        var result = await _userService.Update(request);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldRemoveUser_AndRelatedEntities()
    {
        //Arrange
        var user = await _fixture.CreateUser();

        //Act
        var result = await _userService.Delete(user.Id);

        //Assert
        result.Should().BeTrue();
    }
}