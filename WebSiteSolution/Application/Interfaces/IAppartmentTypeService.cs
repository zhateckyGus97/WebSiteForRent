using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppartmentTypeService
    {
        public Task Add(AppartmentTypeDTO appartmentType);
        public Task Delete(int id);
        public Task<AppartmentTypeDTO> GetById(int id);
        public Task<IEnumerable<AppartmentTypeDTO>> GetAll();
        public Task Update(AppartmentTypeDTO appartmentType);
    }
}
