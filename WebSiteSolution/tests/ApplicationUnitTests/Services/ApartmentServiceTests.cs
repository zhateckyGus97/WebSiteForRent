using Application.Services;
using AutoMapper;
using Infrastructure.Interfaces;
using Moq;
using Application.Mapping;
using FluentAssertions;
using Domain.Entities;
using Bogus;
using Application.Exceptions;
using Application.Requests.ApartmentRequests;
using Application.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class ApartmentServiceTests
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IApartmentService _apartmentService;
        private Mock<IApartmentRepository> _apartmentRepositoryMock;
        private readonly ILogger<IApartmentService> _logger;
        private Faker _faker;

        public ApartmentServiceTests()
        {
            _faker = new Faker();

            _apartmentRepositoryMock = new Mock<IApartmentRepository>();
            _apartmentRepository = _apartmentRepositoryMock.Object;

            var dealRepositoryMock = new Mock<IDealRepository>();
            _dealRepository = dealRepositoryMock.Object;

            var reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviewRepository = reviewRepositoryMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<IApartmentService>.Instance;

            _apartmentService = new ApartmentService(_apartmentRepository, _reviewRepository, _dealRepository, _mapper, _logger);
        }

        [Fact]
        public void ShouldBeAvailableToCreate()
        {
            _apartmentService.Should().NotBeNull();
        }

        [Fact]
        public async void Add_ValidRequest_ReturnsId()
        {
            // Arrange
            var apartmentId = _faker.Random.Int(1, 100);
            var request = new CreateApartmentRequest
            {
                OwnerId = _faker.Random.Int(1, 100),
                Title = _faker.Lorem.Sentence(),
                Description = _faker.Lorem.Paragraph(),
                Address = _faker.Address.FullAddress(),
                PricePerDay = _faker.Random.Double(50, 500),
                NumOfFloor = _faker.Random.Int(1, 20),
                Square = _faker.Random.Double(30, 200),
                Capacity = _faker.Random.Int(1, 10)
            };

            _apartmentRepositoryMock.Setup(x => x.Create(It.Is<Apartment>(a =>
                    a.OwnerId == request.OwnerId &&
                    a.Title == request.Title &&
                    a.Description == request.Description &&
                    a.Address == request.Address &&
                    a.PricePerDay == request.PricePerDay &&
                    a.NumOfFloor == request.NumOfFloor &&
                    a.Square == request.Square &&
                    a.Capacity == request.Capacity)))
                .ReturnsAsync(apartmentId);

            // Act
            var result = await _apartmentService.Add(request);

            // Assert
            result.Should().Be(apartmentId);
            _apartmentRepositoryMock.Verify(x => x.Create(It.Is<Apartment>(a =>
                    a.OwnerId == request.OwnerId &&
                    a.Title == request.Title &&
                    a.Description == request.Description &&
                    a.Address == request.Address &&
                    a.PricePerDay == request.PricePerDay &&
                    a.NumOfFloor == request.NumOfFloor &&
                    a.Square == request.Square &&
                    a.Capacity == request.Capacity)), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnApartment()
        {
            // Arrange
            var apartment = new Apartment()
            {
                Id = 1,
                OwnerId = _faker.Random.Int(1, 100),
                Title = _faker.Lorem.Sentence(),
                Description = _faker.Lorem.Paragraph(),
                Address = _faker.Address.FullAddress(),
                PricePerDay = _faker.Random.Double(50, 500),
                NumOfFloor = _faker.Random.Int(1, 20),
                Square = _faker.Random.Double(30, 200),
                Capacity = _faker.Random.Int(1, 10)
            };

            _apartmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(apartment);

            // Act
            var result = await _apartmentService.GetById(apartment.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(apartment.Id);
            result.Title.Should().Be(apartment.Title);
            result.PricePerDay.Should().Be(apartment.PricePerDay);
            _apartmentRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetById_NonExistingApartment_ThrowsNotFoundException()
        {
            // Arrange
            var apartmentId = _faker.Random.Int(1, 100);
            _apartmentRepositoryMock.Setup(x => x.GetById(apartmentId))
                .ReturnsAsync((Apartment)null);

            // Act + Assert
            await _apartmentService.Invoking(x => x.GetById(apartmentId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage($"Apartment with id {apartmentId} not found!");
        }

        [Fact]
        public async void GetAll_WithApartments_ReturnsApartments()
        {
            //Arrange
            var apartments = new List<Apartment>()
            {
                new Apartment { Id = 1, Title = "Luxury Apartment", PricePerDay = 200 },
                new Apartment { Id = 2, Title = "Cozy Studio", PricePerDay = 100 },
                new Apartment { Id = 3, Title = "Family House", PricePerDay = 300 }
            };

            _apartmentRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(apartments);

            //Act
            var result = await _apartmentService.GetAll();

            //Assert
            result.Should().HaveCount(3);
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(3);
        }

        [Fact]
        public async void Update_ValidRequest_UpdateApartment()
        {
            // Arrange
            var request = new UpdateApartmentRequest
            {
                Id = 1,
                OwnerId = _faker.Random.Int(1, 100),
                Title = _faker.Lorem.Sentence(),
                Description = _faker.Lorem.Paragraph(),
                Address = _faker.Address.FullAddress(),
                PricePerDay = _faker.Random.Double(50, 500),
                NumOfFloor = _faker.Random.Int(1, 20),
                Square = _faker.Random.Double(30, 200),
                Capacity = _faker.Random.Int(1, 10)
            };

            _apartmentRepositoryMock.Setup(x => x.Update(It.IsAny<Apartment>())).ReturnsAsync(true);

            // Act
            await _apartmentService.Update(request);

            // Assert
            _apartmentRepositoryMock.Verify(x => x.Update(It.Is<Apartment>(a =>
                    a.Id == request.Id &&
                    a.OwnerId == request.OwnerId &&
                    a.Title == request.Title &&
                    a.Description == request.Description &&
                    a.Address == request.Address &&
                    a.PricePerDay == request.PricePerDay &&
                    a.NumOfFloor == request.NumOfFloor &&
                    a.Square == request.Square &&
                    a.Capacity == request.Capacity)), Times.Once);
        }

        [Fact]
        public async void Delete_ExistingApartment_DeletesRelatedEntities()
        {
            // Arrange
            var apartmentId = _faker.Random.Int(1, 100);
            var apartment = new Apartment { Id = apartmentId };

            _apartmentRepositoryMock.Setup(x => x.GetById(apartmentId)).ReturnsAsync(apartment);
            _apartmentRepositoryMock.Setup(x => x.Delete(apartmentId)).ReturnsAsync(true);

            // Act
            var result = await _apartmentService.Delete(apartmentId);

            // Assert
            result.Should().BeTrue();
            _apartmentRepositoryMock.Verify(x => x.Delete(apartmentId), Times.Once);
        }

        [Fact]
        public async void Delete_NonExistingApartment_ThrowsNotFoundException()
        {
            // Arrange
            var apartmentId = _faker.Random.Int(1, 100);
            _apartmentRepositoryMock.Setup(x => x.GetById(apartmentId))
                .ReturnsAsync((Apartment)null);

            // Act + Assert
            await _apartmentService.Invoking(x => x.Delete(apartmentId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage($"Apartment with id {apartmentId} not found!");
        }
    }
}