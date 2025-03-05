using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        public Task<UserInfo> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserInfo>> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}
