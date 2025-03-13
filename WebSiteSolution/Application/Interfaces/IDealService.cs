using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDealService
    {
        public Task Add(DealDTO deal);
        public Task<bool> Delete(int id);
        public Task<DealDTO> GetById(int id);
        public Task<IEnumerable<DealDTO>> GetAll();
        public Task<bool> Update(DealDTO deal);
    }
}
