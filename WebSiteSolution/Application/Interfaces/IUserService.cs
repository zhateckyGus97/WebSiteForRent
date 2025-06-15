using Application.Requests.UserRequests;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IUserService
    {
        public Task<int> Add(RegistrationUserRequest user);
        public Task<bool> Delete(int id);
        public Task<UserResponse> GetById(int id);
        public Task<UserResponse> GetByEmail(string email);
        public Task<IEnumerable<UserResponse>> GetAll();
        public Task<bool> Update(UpdateUserRequest user);
    }
}
