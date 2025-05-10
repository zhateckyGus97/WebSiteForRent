using Application.Services;
using AutoMapper;
using Infrastructure.Interfaces;
using Moq;
using Application.Mapping;
using FluentAssertions;
using Domain.Entities;
using Bogus;
using Application.Exceptions;
using Application.Requests.DealRequests;
using Application.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class DealServiceTests
    {
        private readonly IDealRepository _dealRepository;
        private readonly IMapper _mapper;
        private readonly IDealService _dealService;
        private Mock<IDealRepository> _dealRepositoryMock;
        private readonly ILogger<IDealService> _logger;
        private Faker _faker;

        public DealServiceTests()
        {
            _faker = new Faker();

            _dealRepositoryMock = new Mock<IDealRepository>();
            _dealRepository = _dealRepositoryMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<IDealService>.Instance;

            _dealService = new DealService(_dealRepository, _mapper, _logger);
        }

        [Fact]
        public void ShouldBeAvailableToCreate()
        {
            _dealService.Should().NotBeNull();
        }

        [Fact]
        public async void Add_ValidRequest_ReturnsId()
        {
            // Arrange
            var dealId = _faker.Random.Int(1, 100);
            var request = new CreateDealRequest
            {
                UserId = _faker.Random.Int(1, 100),
                ApartmentId = _faker.Random.Int(1, 100),
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(7),
                TotalPrice = _faker.Random.Double(100, 1000)
            };

            _dealRepositoryMock.Setup(x => x.Create(It.Is<Deal>(d =>
                    d.UserId == request.UserId &&
                    d.ApartmentId == request.ApartmentId &&
                    d.CheckInDate == request.CheckInDate &&
                    d.CheckOutDate == request.CheckOutDate &&
                    d.TotalPrice == request.TotalPrice)))
                .ReturnsAsync(dealId);

            // Act
            var result = await _dealService.Add(request);

            // Assert
            result.Should().Be(dealId);
            _dealRepositoryMock.Verify(x => x.Create(It.Is<Deal>(d =>
                    d.UserId == request.UserId &&
                    d.ApartmentId == request.ApartmentId &&
                    d.CheckInDate == request.CheckInDate &&
                    d.CheckOutDate == request.CheckOutDate &&
                    d.TotalPrice == request.TotalPrice)), Times.Once);
        }

        
        [Fact]
        public async void GetById_ShouldReturnDealWithCorrectMapping()
        {
            // Arrange
            var deal = new Deal()
            {
                Id = 1,
                UserId = 2,
                ApartmentId = 3,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(7),
                TotalPrice = 1000,
                CreatedAt = DateTime.Now.AddHours(-1),
                UpdatedAt = DateTime.Now
            };

            _dealRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(deal);

            // Act
            var result = await _dealService.GetById(deal.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(deal.Id);
            result.UserId.Should().Be(deal.UserId);
            result.ApartmentId.Should().Be(deal.ApartmentId);
            result.CheckInDate.Should().Be(deal.CheckInDate);
            result.CheckOutDate.Should().Be(deal.CheckOutDate);
            result.TotalPrice.Should().Be(deal.TotalPrice);
            _dealRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetAll_ShouldReturnDealsWithTimestamps()
        {
            // Arrange
            var deals = new List<Deal>()
            {
                new Deal {
                    Id = 1,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    UpdatedAt = DateTime.Now.AddDays(-1)
                },
                new Deal {
                    Id = 2,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    UpdatedAt = DateTime.Now
                }
            };

            _dealRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(deals);

            // Act
            var result = await _dealService.GetAll();

            // Assert
            result.Should().HaveCount(2);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
            _dealRepositoryMock.Verify(x => x.GetAll(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallRepositoryWithCorrectData()
        {
            // Arrange
            var originalDeal = new Deal
            {
                Id = 1,
                UserId = 2,
                ApartmentId = 3,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(3),
                TotalPrice = 500,
                CreatedAt = DateTime.Now.AddDays(-5),
                UpdatedAt = DateTime.Now.AddDays(-1)
            };

            var request = new UpdateDealRequest
            {
                Id = 1,
                CheckInDate = DateTime.Now.AddDays(2),
                CheckOutDate = DateTime.Now.AddDays(4),
                TotalPrice = 750
            };

            _dealRepositoryMock.Setup(x => x.GetById(1)).ReturnsAsync(originalDeal);
            _dealRepositoryMock.Setup(x => x.Update(It.IsAny<Deal>())).ReturnsAsync(true);

            // Act
            await _dealService.Update(request);

            // Assert
            _dealRepositoryMock.Verify(x => x.Update(It.IsAny<Deal>()), Times.Once);

            _dealRepositoryMock.Verify(x => x.Update(It.Is<Deal>(d =>
                d.Id == 1
            )), Times.Once);

            _dealRepositoryMock.Verify(x => x.Update(It.Is<Deal>(d =>
                d.CheckInDate == request.CheckInDate
            )), Times.Once);

            _dealRepositoryMock.Verify(x => x.Update(It.Is<Deal>(d =>
                d.CheckOutDate == request.CheckOutDate
            )), Times.Once);
        }

        [Fact]
        public async void Delete_ShouldVerifyDealExistence()
        {
            // Arrange
            var dealId = 1;
            var deal = new Deal { Id = dealId };

            _dealRepositoryMock.Setup(x => x.GetById(dealId)).ReturnsAsync(deal);
            _dealRepositoryMock.Setup(x => x.Delete(dealId)).ReturnsAsync(true);

            // Act
            var result = await _dealService.Delete(dealId);

            // Assert
            result.Should().BeTrue();
            _dealRepositoryMock.Verify(x => x.GetById(dealId), Times.Once);
            _dealRepositoryMock.Verify(x => x.Delete(dealId), Times.Once);
        }
    }
}