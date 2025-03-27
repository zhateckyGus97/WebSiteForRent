using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetById(int id);
        public Task<IEnumerable<User>> GetAll();
        public Task<int> Create(User user);
        public Task<bool> Update(User user);
        public Task<bool> Delete(int id);
    }
}