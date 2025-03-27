using Application.DTO;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        public Task<int> Add(ReviewDTO review);
        public Task<bool> Delete(int id);
        public Task<ReviewDTO> GetById(int id);
        public Task<IEnumerable<ReviewDTO>> GetAll();
        public Task<bool> Update(ReviewDTO review);
    }
}
