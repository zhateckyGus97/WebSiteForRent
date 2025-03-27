using Application.DTO;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<int> Add(UserDTO user);
        public Task<bool> Delete(int id);
        public Task<UserDTO> GetById(int id);
        public Task<IEnumerable<UserDTO>> GetAll();
        public Task<bool> Update(UserDTO user);
    }
}
