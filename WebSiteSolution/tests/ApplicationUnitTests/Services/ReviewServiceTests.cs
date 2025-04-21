using Application.Services;
using AutoMapper;
using Infrastructure.Interfaces;
using Moq;
using Application.Mapping;
using FluentAssertions;
using Domain.Entities;
using Bogus;
using Application.Exceptions;
using Application.Requests.ReviewRequests;
using Application.Responses;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ApplicationUnitTests.Services
{
    public class ReviewServiceTests
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IReviewService _reviewService;
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly ILogger<IReviewService> _logger;
        private Faker _faker;

        public ReviewServiceTests()
        {
            _faker = new Faker();

            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviewRepository = _reviewRepositoryMock.Object;

            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = mappingConfig.CreateMapper();

            _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<IReviewService>.Instance;

            _reviewService = new ReviewService(_reviewRepository, _mapper, _logger);
        }

        [Fact]
        public void ShouldBeAvailableToCreate()
        {
            _reviewService.Should().NotBeNull();
        }

        [Fact]
        public async void Add_ValidRequest_ReturnsId()
        {
            // Arrange
            var reviewId = _faker.Random.Int(1, 100);
            var request = new CreateReviewRequest
            {
                UserId = 1233,//_faker.Random.Int(1, 100),
                ApartmentId = 600,//_faker.Random.Int(1, 100),
                Rating = _faker.Random.Int(1, 5),
                Comment = _faker.Lorem.Sentence()
            };

            _reviewRepositoryMock.Setup(x => x.Create(It.Is<Review>(r =>
                    r.UserId == request.UserId &&
                    r.ApartmentId == request.ApartmentId &&
                    r.Rating == request.Rating &&
                    r.Comment == request.Comment)))
                .ReturnsAsync(reviewId);

            // Act
            var result = await _reviewService.Add(request);

            // Assert
            result.Should().Be(reviewId);
            _reviewRepositoryMock.Verify(x => x.Create(It.Is<Review>(r =>
                    r.UserId == request.UserId &&
                    r.ApartmentId == request.ApartmentId &&
                    r.Rating == request.Rating &&
                    r.Comment == request.Comment)), Times.Once);
        }

        [Fact]
        public async void GetById_ShouldReturnReview()
        {
            // Arrange
            var review = new Review()
            {
                Id = 1,
                UserId = _faker.Random.Int(1, 100),
                ApartmentId = _faker.Random.Int(1, 100),
                Rating = _faker.Random.Int(1, 5),
                Comment = _faker.Lorem.Sentence(),
                CreatedAt = DateTime.Now
            };

            _reviewRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(review);

            // Act
            var result = await _reviewService.GetById(review.Id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(review.Id);
            result.Rating.Should().Be(review.Rating);
            _reviewRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void GetById_NonExistingReview_ThrowsNotFoundException()
        {
            // Arrange
            var reviewId = _faker.Random.Int(1, 100);
            _reviewRepositoryMock.Setup(x => x.GetById(reviewId))
                .ReturnsAsync((Review)null);

            // Act + Assert
            await _reviewService.Invoking(x => x.GetById(reviewId))
                .Should().ThrowAsync<NotFoundApplicationException>()
                .WithMessage($"Review with id {reviewId} not found!");
        }

        [Fact]
        public async void Update_ValidRequest_UpdateReview()
        {
            // Arrange
            var existingReview = new Review()
            {
                Id = 1, // Явно задаём Id
                UserId = _faker.Random.Int(1, 100),
                ApartmentId = _faker.Random.Int(1, 100),
                Rating = 3,
                Comment = "Original comment",
                CreatedAt = DateTime.Now
            };

            var request = new UpdateReviewRequest
            {
                Id = existingReview.Id, // Используем тот же Id
                Rating = 5, // Фиксируем значение для точной проверки
                Comment = "Updated comment"
            };

            _reviewRepositoryMock.Setup(x => x.Update(It.IsAny<Review>()))
                .ReturnsAsync(true);

            // Act
            await _reviewService.Update(request);

            // Assert
            _reviewRepositoryMock.Verify(x => x.Update(It.Is<Review>(r =>
                r.Id == existingReview.Id &&
                r.Rating == request.Rating &&
                r.Comment == request.Comment)),
            Times.Once);
        }

        [Fact]
        public async void Delete_ExistingReview_ReturnsTrue()
        {
            // Arrange
            var reviewId = _faker.Random.Int(1, 100);
            var review = new Review { Id = reviewId };

            _reviewRepositoryMock.Setup(x => x.GetById(reviewId)).ReturnsAsync(review);
            _reviewRepositoryMock.Setup(x => x.Delete(reviewId)).ReturnsAsync(true);

            // Act
            var result = await _reviewService.Delete(reviewId);

            // Assert
            result.Should().BeTrue();
            _reviewRepositoryMock.Verify(x => x.Delete(reviewId), Times.Once);
        }
    }
}