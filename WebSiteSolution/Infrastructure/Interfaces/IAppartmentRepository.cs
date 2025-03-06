using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IAppartmentRepository
    {
        public Task<Appartment?> GetById(int id);
        public Task<IEnumerable<Appartment>> GetAll();
        public Task Create(Appartment appartment);
        public Task Update(Appartment appartment);
        public Task Delete(Appartment appartment);
    }
}
