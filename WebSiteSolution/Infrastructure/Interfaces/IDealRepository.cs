using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
