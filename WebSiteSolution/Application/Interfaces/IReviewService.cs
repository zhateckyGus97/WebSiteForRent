using Application.DTO;
using Application.Requests.ReviewRequests;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        public Task<int> Add(CreateReviewRequest review);
        public Task<bool> Delete(int id);
        public Task<CreateReviewRequest> GetById(int id);
        public Task<IEnumerable<CreateReviewRequest>> GetAll();
        public Task<bool> Update(UpdateReviewRequest review);
    }
}
