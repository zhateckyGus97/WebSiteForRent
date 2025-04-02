using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.ReviewRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateReviewRequest review)
        {
            var reviews = await _reviewRepository.GetAll();
            if (reviews.Select(x => x.Id == review.Id).ToList().Count() > 0)
            {
                throw new KeyAlreadyExistsException($"Id with value {review.Id} already exists!");
            }

            var mappedReview = _mapper.Map<Review>(review);
            if (mappedReview == null)
            {
                return -1;
                
            }

            await _reviewRepository.Create(mappedReview);
            return mappedReview.Id;
        }

        public async Task<bool> Delete(int id)
        {
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
            if(review == null)
            {
                throw new NotFoundApplicationException($"Review with id {id} not found!");
            }
            
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<bool> Update(UpdateReviewRequest review)
        {
            if (review == null)
            {
                return false;
            }

            var mappedReview = _mapper.Map<Review>(review);
            return await _reviewRepository.Update(mappedReview);
        }
    }
}
