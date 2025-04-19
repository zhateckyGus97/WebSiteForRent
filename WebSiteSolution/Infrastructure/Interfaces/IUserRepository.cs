using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(int id);
        Task<User?> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task<int> Create(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);
    }
}