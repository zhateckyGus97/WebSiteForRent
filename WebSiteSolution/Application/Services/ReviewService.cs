using Application.DTO;
using Application.Interfaces;
using Application.Requests.ReviewRequests;
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
            var mappedReview = _mapper.Map<Review>(review);
            if (mappedReview != null)
            {
                await _reviewRepository.Create(mappedReview);
                return mappedReview.Id;
            }

            return -1;
        }

        public async Task<bool> Delete(int id)
        {
            return await _reviewRepository.Delete(id);
        }

        public async Task<IEnumerable<CreateReviewRequest>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();
            var mappedReviews = reviews.Select(r => _mapper.Map<CreateReviewRequest>(r)).ToList();
            return mappedReviews;
        }

        public async Task<CreateReviewRequest> GetById(int id)
        {
            var review = await _reviewRepository.GetById(id);
            var mappedReview = _mapper.Map<CreateReviewRequest>(review);
            return mappedReview;
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
