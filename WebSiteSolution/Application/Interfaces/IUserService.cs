using Application.Requests.UserReauests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<int> Add(CreateUserRequest user);
        public Task<bool> Delete(int id);
        public Task<UserResponse> GetById(int id);
        public Task<IEnumerable<UserResponse>> GetAll();
        public Task<bool> Update(UpdateUserRequest user);
    }
}
