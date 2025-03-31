using Application.DTO;
using Application.Requests.DealRequests;

namespace Application.Interfaces
{
    public interface IDealService
    {
        public Task<int> Add(CreateDealRequest deal);
        public Task<bool> Delete(int id);
        public Task<CreateDealRequest> GetById(int id);
        public Task<IEnumerable<CreateDealRequest>> GetAll();
        public Task<bool> Update(UpdateDealRequest deal);
    }
}
