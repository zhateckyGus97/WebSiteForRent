using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IApartmentRepository
    {
        public Task<Apartment?> GetById(int id);
        public Task<IEnumerable<Apartment>> GetAll();
        public Task<int> Create(Apartment Apartment);
        public Task<bool> Update(Apartment Apartment);
        public Task<bool> Delete(int Id);
    }
}
