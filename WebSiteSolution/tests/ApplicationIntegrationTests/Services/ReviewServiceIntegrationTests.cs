using Application.Interfaces;
using Application.Requests.ReviewRequests;
using ApplicationIntegrationTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;


public class ReviewServiceTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture _fixture;
    private readonly IReviewService _reviewService;

    public ReviewServiceTests(TestingFixture fixture)
    {
        _fixture = fixture;
        _reviewService = fixture.ServiceProvider.GetRequiredService<IReviewService>();
    }

    [Fact]
    public async Task Add_ShouldCreateReview_AndReturnId()
    {
        var user = await _fixture.CreateUser();
        var apartment = await _fixture.CreateApartment();
        var request = new CreateReviewRequest
        {
            UserId = user.Id,
            ApartmentId = apartment.Id,
            Rating = 5,
            Comment = "Great apartment!"
        };

        var reviewId = await _reviewService.Add(request);
        reviewId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetById_ShouldReturnReview_WhenExists()
    {
        var review = await _fixture.CreateReview();
        var result = await _reviewService.GetById(review.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(review.Id);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllReviews()
    {
        await _fixture.CreateReview();
        await _fixture.CreateReview();

        var result = await _reviewService.GetAll();
        result.Count().Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public async Task Update_ShouldModifyReview()
    {
        var review = await _fixture.CreateReview();
        var request = new UpdateReviewRequest
        {
            Id = review.Id,
            Rating = 4,
            Comment = "Updated comment"
        };

        var result = await _reviewService.Update(request);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldRemoveReview()
    {
        var review = await _fixture.CreateReview();
        var result = await _reviewService.Delete(review.Id);
        result.Should().BeTrue();
    }
}