using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> Add(ReviewDTO review)
        {
            var mappedReview = _mapper.Map<Review>(review);
            if(mappedReview != null)
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

        public async Task<IEnumerable<ReviewDTO>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();
            var mappedReviews = reviews.Select(r => _mapper.Map<ReviewDTO>(r)).ToList();
            return mappedReviews;
        }

        public async Task<ReviewDTO> GetById(int id)
        {
            var review = await _reviewRepository.GetById(id);
            var mappedReview = _mapper.Map<ReviewDTO>(review);
            return mappedReview;
        }

        public async Task<bool> Update(ReviewDTO review)
        {
            var mappedReview = _mapper.Map<Review>(review);
            return await _reviewRepository.Update(mappedReview);
        }
    }
}
