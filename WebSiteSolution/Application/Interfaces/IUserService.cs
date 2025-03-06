using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task Add(UserDTO user);
        public Task Delete(int id);
        public Task<UserDTO> GetById(int id);
        public Task<IEnumerable<UserDTO>> GetAll();
        public Task Update(UserDTO user);
    }
}
