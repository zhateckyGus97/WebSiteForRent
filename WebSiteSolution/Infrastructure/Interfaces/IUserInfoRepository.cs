using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserInfoRepository
    {
        public Task<UserInfo> GetById(int id);
        public Task<IEnumerable<UserInfo>> ReadAll();
    }
}
