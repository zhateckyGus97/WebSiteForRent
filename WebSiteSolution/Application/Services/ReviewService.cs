using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.ReviewRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper, ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateReviewRequest review)
        {
            var mappedReview = _mapper.Map<Review>(review);
            var result = await _reviewRepository.Create(mappedReview);
            _logger.LogInformation($"Review was created with id: {result}");
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var review = await _reviewRepository.GetById(id);
            if (review == null)
                throw new NotFoundApplicationException($"Review with id {id} not found!");

            _logger.LogInformation($"Review with id: {id} was deleted");
            return await _reviewRepository.Delete(id);
        }

        public async Task<IEnumerable<ReviewResponse>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();
            var mappedReviews = reviews.Select(r => _mapper.Map<ReviewResponse>(r)).ToList();
            return mappedReviews;
        }

        public async Task<ReviewResponse> GetById(int id)
        {
            var review = await _reviewRepository.GetById(id);
            if (review == null)
            {
                throw new NotFoundApplicationException($"Review with id {id} not found!");
            }
            
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<bool> Update(UpdateReviewRequest review)
        {
            var mappedReview = _mapper.Map<Review>(review);

            if (!await _reviewRepository.Update(mappedReview))
                throw new EntityUpdateException("Review wasn't updated!");

            _logger.LogInformation($"Review with id: {review.Id} was updated");
            return true;
        }
    }
}
