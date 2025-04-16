using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IApartmentRepository
    {
        public Task<Apartment?> GetById(int id);
        public Task<IEnumerable<Apartment>> GetAll();
        public Task<int> Create(Apartment Apartment);
        public Task<bool> Update(Apartment Apartment);
        public Task<bool> Delete(int id);
        public Task DeleteByOwnerId(int ownerid);
    }
}
