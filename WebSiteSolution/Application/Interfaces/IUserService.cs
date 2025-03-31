using Application.DTO;
using Application.Requests.UserReauests;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<int> Add(CreateUserRequest user);
        public Task<bool> Delete(int id);
        public Task<CreateUserRequest> GetById(int id);
        public Task<IEnumerable<CreateUserRequest>> GetAll();
        public Task<bool> Update(UpdateUserRequest user);
    }
}
