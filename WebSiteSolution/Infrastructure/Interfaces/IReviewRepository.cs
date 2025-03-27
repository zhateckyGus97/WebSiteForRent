using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IReviewRepository
    {
        public Task<Review?> GetById(int id);
        public Task<IEnumerable<Review>> GetAll();
        public Task<int> Create(Review review);
        public Task<bool> Update(Review review);
        public Task<bool> Delete(int id);
    }
}
