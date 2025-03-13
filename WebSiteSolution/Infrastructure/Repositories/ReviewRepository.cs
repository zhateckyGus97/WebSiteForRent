using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly List<Review> _reviews = new List<Review>();

        public Task<int> Create(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review));

            _reviews.Add(review);
            return Task.FromResult(review.Id);
        }

        public Task<bool> Delete(int Id)
        {
            var review = _reviews.FirstOrDefault(r =>  r.Id == Id);

            if (review == null)
                return Task.FromResult(false);

            _reviews.Remove(review);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Review>> GetAll()
        {
            return Task.FromResult(_reviews.AsEnumerable());
        }

        public Task<Review?> GetById(int id)
        {
            var review = _reviews.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(review);
        }

        public Task<bool> Update(Review review)
        {
            if(review == null)
                throw new ArgumentNullException(nameof(review));

            var old_review = _reviews.FirstOrDefault(r => r.Id == review.Id);

            if(old_review == null)
                return Task.FromResult(false);

            old_review.ApartmentId = review.ApartmentId;
            old_review.UserId = review.UserId;
            old_review.Rating = review.Rating;
            old_review.Comment = review.Comment;
            old_review.UpdatedAt = DateTime.Now;

            return Task.FromResult(true);
        }
    }
}
