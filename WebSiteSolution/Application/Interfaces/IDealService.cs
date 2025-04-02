using Application.DTO;
using Application.Requests.DealRequests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IDealService
    {
        public Task<int> Add(CreateDealRequest deal);
        public Task<bool> Delete(int id);
        public Task<DealResponse> GetById(int id);
        public Task<IEnumerable<DealResponse>> GetAll();
        public Task<bool> Update(UpdateDealRequest deal);
    }
}
