using Application.Interfaces;
using Application.Requests.ApartmentRequests;
using ApplicationIntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

public class ApartmentServiceTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture _fixture;
    private readonly IApartmentService _apartmentService;

    public ApartmentServiceTests(TestingFixture fixture)
    {
        _fixture = fixture;
        _apartmentService = fixture.ServiceProvider.GetRequiredService<IApartmentService>();
    }

    [Fact]
    public async Task Add_ShouldCreateApartment_AndReturnId()
    {
        //Arrange
        var user = await _fixture.CreateUser();
        var request = new CreateApartmentRequest
        {
            OwnerId = user.Id,
            Title = "Test Apartment",
            Description = "Test Description",
            Address = "Test Address",
            PricePerDay = 100,
            NumOfFloor = 2,
            Square = 50,
            Capacity = 4
        };

        //Act
        var apartmentId = await _apartmentService.Add(request);

        //Assert
        apartmentId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnApartment_WhenExists()
    {
        //Arrange
        var apartment = await _fixture.CreateApartment();

        //Act
        var result = await _apartmentService.GetById(apartment.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(apartment.Id);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllApartments()
    {
        //Arrange
        await _fixture.CreateApartment();
        await _fixture.CreateApartment();

        //Act
        var result = await _apartmentService.GetAll();

        //Assert
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyApartment()
    {
        //Arrange
        var apartment = await _fixture.CreateApartment();
        var request = new UpdateApartmentRequest
        {
            Id = apartment.Id,
            Title = "Updated Title",
            Description = "Updated Description",
            Address = "Updated Address",
            PricePerDay = 150,
            NumOfFloor = 3,
            Square = 60,
            Capacity = 5
        };

        //Act
        var result = await _apartmentService.Update(request);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldRemoveApartment_AndRelatedEntities()
    {
        //Arrange
        var apartment = await _fixture.CreateApartment();

        //Act
        var result = await _apartmentService.Delete(apartment.Id);

        //Assert
        result.Should().BeTrue();
    }
}