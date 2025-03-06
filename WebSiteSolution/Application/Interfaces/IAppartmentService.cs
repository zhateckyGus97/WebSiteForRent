using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppartmentService
    {
        public Task Add(AppartmentDTO appartment);
        public Task Delete(int id);
        public Task<AppartmentDTO> GetById(int id);
        public Task<IEnumerable<AppartmentDTO>> GetAll();
        public Task Update(AppartmentDTO appartment);
    }
}
