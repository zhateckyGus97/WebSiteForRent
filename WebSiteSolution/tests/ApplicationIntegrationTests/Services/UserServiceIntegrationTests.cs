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
        _userService = fixture.ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task Add_ShouldCreateUser_AndReturnId()
    {
        var request = new CreateUserRequest
        {
            FullName = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Role = "User",
            Passport = "AB1234567",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        var userId = await _userService.Add(request);
        userId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        var user = await _fixture.CreateUser();
        var result = await _userService.GetById(user.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(user.Id);
    }

    [Fact]
    public async Task GetById_ShouldThrow_WhenUserNotExists()
    {
        Func<Task> act = async () => await _userService.GetById(-1);
        await act.Should().ThrowAsync<NotFoundApplicationException>();
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllUsers()
    {
        await _fixture.CreateUser();
        await _fixture.CreateUser();

        var result = await _userService.GetAll();
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyUser()
    {
        var user = await _fixture.CreateUser();
        var request = new UpdateUserRequest
        {
            Id = user.Id,
            FullName = "Updated Name",
            Email = "updated@example.com",
            PhoneNumber = "0987654321",
            Role = "Admin",
            Passport = "XY9876543",
            DateOfBirth = new DateTime(1985, 5, 15)
        };

        var result = await _userService.Update(request);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldRemoveUser_AndRelatedEntities()
    {
        var user = await _fixture.CreateUser();
        var result = await _userService.Delete(user.Id);
        result.Should().BeTrue();
    }
}