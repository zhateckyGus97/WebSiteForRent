using Application.Interfaces;
using Application.Requests.DealRequests;
using ApplicationIntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

public class DealServiceIntegrationTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture _fixture;
    private readonly IDealService _dealService;

    public DealServiceIntegrationTests(TestingFixture fixture)
    {
        _fixture = fixture;
        _dealService = fixture.ServiceProvider.GetRequiredService<IDealService>();
    }

    [Fact]
    public async Task Add_ShouldCreateDeal_AndReturnId()
    {
        //Arrange
        var user = await _fixture.CreateUser();
        var apartment = await _fixture.CreateApartment();
        var request = new CreateDealRequest
        {
            UserId = user.Id,
            ApartmentId = apartment.Id,
            CheckInDate = DateTime.Now.AddDays(1),
            CheckOutDate = DateTime.Now.AddDays(5),
            TotalPrice = 1000
        };

        //Act
        var dealId = await _dealService.Add(request);

        //Assert
        dealId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnDeal_WhenExists()
    {
        //Arrange
        var deal = await _fixture.CreateDeal();
        //Act
        var result = await _dealService.GetById(deal.Id);

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(deal.Id);
        result.UserId.Should().Be(deal.UserId);
        result.CheckInDate.Should().Be(deal.CheckInDate);
        result.CheckOutDate.Should().Be(deal.CheckOutDate);
        result.ApartmentId.Should().Be(deal.ApartmentId);
        result.TotalPrice.Should().Be(deal.TotalPrice);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllDeals()
    {
        //Arrange
        await _fixture.CreateDeal();
        await _fixture.CreateDeal();

        //Act
        var result = await _dealService.GetAll();

        //Assert
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyDeal()
    {
        //Arrange
        var deal = await _fixture.CreateDeal();
        var request = new UpdateDealRequest
        {
            Id = deal.Id,
            UserId = deal.UserId,
            ApartmentId = deal.ApartmentId,
            CheckInDate = DateTime.Now.AddDays(2),
            CheckOutDate = DateTime.Now.AddDays(6),
            TotalPrice = 1200
        };

        //Act
        var result = await _dealService.Update(request);

        //Assert
        result.Should().BeTrue();
        request.CheckInDate.Should().NotBe(deal.CheckInDate);
        request.CheckOutDate.Should().NotBe(deal.CheckOutDate);
        request.TotalPrice.Should().NotBe(deal.TotalPrice);
    }

    [Fact]
    public async Task Delete_ShouldRemoveDeal()
    {
        //Arrange
        var deal = await _fixture.CreateDeal();

        //Act
        var result = await _dealService.Delete(deal.Id);

        //Assert
        result.Should().BeTrue();
    }
}