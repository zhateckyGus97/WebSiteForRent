using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAppartmentTypeRepository
    {
        public Task<AppartmentType?> GetById(int id);
        public Task<IEnumerable<AppartmentType>> GetAll();
        public Task Create(AppartmentType appType);
        public Task Update(AppartmentType appType);
        public Task Delete(AppartmentType appType);
    }
}
