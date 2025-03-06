using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;

namespace Infrastructure.Repositories
{
    public class UserRepository(Application.AppContext context) : IUserRepository
    {
        public Task<User?> GetById(int id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(user);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            return Task.FromResult<IEnumerable<User>>(context.Users);
        }

        public Task Create(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
