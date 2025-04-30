using Domain.Entities;
using Infrastructure.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories.InMemoryRepositories
{
    [ExcludeFromCodeCoverage]
    public class UserInMemoryRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserInMemoryRepository()
        {
            _users = new List<User>()
            {
                new User{Id = 1, FullName = "Ilya Gusev", Email = "www@123", PhoneNumber = "22-22-22", Role = "admin", Passport = "77-123", DateOfBirth = DateTime.Now},
                new User{Id = 2, FullName = "Alex Levshinsky", Email = "www@456", PhoneNumber = "33-33-33", Role ="admin", Passport = "76-123", DateOfBirth = DateTime.Now},
                new User{Id = 3, FullName = "Vladimir Putin", Email = "putin@777", PhoneNumber = "44-44-44", Role = "user", Passport = "77-777", DateOfBirth = DateTime.Now}
            };
        }

        public Task<User?> GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<int> Create(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _users.Add(user);
            return Task.FromResult(user.Id);
        }

        public Task<bool> Update(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var old_user = _users.FirstOrDefault(u => u.Id == user.Id);

            if (old_user == null)
                return Task.FromResult(false);

            old_user.PhoneNumber = user.PhoneNumber;
            old_user.DateOfBirth = user.DateOfBirth;
            old_user.Passport = user.Passport;
            old_user.Role = user.Role;
            old_user.Email = user.Email;
            old_user.FullName = user.FullName;

            return Task.FromResult(true);
        }

        public Task<bool> Delete(int Id)
        {
            var user = _users.FirstOrDefault(x => x.Id == Id);
            if (user == null)
                return Task.FromResult(false);

            _users.Remove(user);
            return Task.FromResult(true);
        }
    }
}
