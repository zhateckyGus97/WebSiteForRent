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
        public Task Create(Deal deal);
        public Task Update(Deal deal);
        public Task Delete(Deal deal);
    }
}
