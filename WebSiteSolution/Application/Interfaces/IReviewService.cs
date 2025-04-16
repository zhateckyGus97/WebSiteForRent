using Application.Requests.ReviewRequests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        public Task<int> Add(CreateReviewRequest review);
        public Task<bool> Delete(int id);
        public Task<ReviewResponse> GetById(int id);
        public Task<IEnumerable<ReviewResponse>> GetAll();
        public Task<bool> Update(UpdateReviewRequest review);
    }
}
