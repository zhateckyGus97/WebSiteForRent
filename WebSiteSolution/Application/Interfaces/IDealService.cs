using Application.DTO;

namespace Application.Interfaces
{
    public interface IDealService
    {
        public Task<int> Add(DealDTO deal);
        public Task<bool> Delete(int id);
        public Task<DealDTO> GetById(int id);
        public Task<IEnumerable<DealDTO>> GetAll();
        public Task<bool> Update(DealDTO deal);
    }
}
