using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IDealRepository
    {
        public Task<Deal?> GetById(int id);
        public Task<IEnumerable<Deal>> GetAll();
        public Task<int> Create(Deal deal);
        public Task<bool> Update(Deal deal);
        public Task<bool> Delete(int id);
    }
}
