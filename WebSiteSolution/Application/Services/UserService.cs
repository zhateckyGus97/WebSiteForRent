using Application.DTO;
using Application.Interfaces;
using Application.Requests.UserReauests;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateUserRequest user)
        {
            var mappedUser = _mapper.Map<User>(user);
            if (mappedUser == null)
            {
                return -1;
            }

            return await _userRepository.Create(mappedUser);
        }

        public async Task<bool> Delete(int id)
        {
            return await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<CreateUserRequest>> GetAll()
        {
            var users = await _userRepository.GetAll();
            var mappedUsers = users.Select(u => _mapper.Map<CreateUserRequest>(u)).ToList();
            return mappedUsers;
        }

        public async Task<CreateUserRequest?> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            var mappedUser = _mapper.Map<CreateUserRequest>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UpdateUserRequest user)
        {
            if (user == null)
            {
                return false;
            }

            var mappedUser = _mapper.Map<User>(user);
            return await _userRepository.Update(mappedUser);
        }
    }
}
