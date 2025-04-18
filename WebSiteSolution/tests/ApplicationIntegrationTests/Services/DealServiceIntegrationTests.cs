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

        var dealId = await _dealService.Add(request);
        dealId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnDeal_WhenExists()
    {
        var deal = await _fixture.CreateDeal();
        var result = await _dealService.GetById(deal.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(deal.Id);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllDeals()
    {
        await _fixture.CreateDeal();
        await _fixture.CreateDeal();

        var result = await _dealService.GetAll();
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyDeal()
    {
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

        var result = await _dealService.Update(request);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldRemoveDeal()
    {
        var deal = await _fixture.CreateDeal();
        var result = await _dealService.Delete(deal.Id);
        result.Should().BeTrue();
    }
}