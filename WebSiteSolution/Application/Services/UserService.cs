using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.UserReauests;
using Application.Responses;
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
            return await _userRepository.Create(mappedUser);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            return await _userRepository.Delete(id);
        }

        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = await _userRepository.GetAll();
            var mappedUsers = users.Select(u => _mapper.Map<UserResponse>(u)).ToList();
            return mappedUsers;
        }

        public async Task<UserResponse> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                throw new NotFoundApplicationException($"User with id {id} not found!");
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<bool> Update(UpdateUserRequest user)
        {
            var mappedUser = _mapper.Map<User>(user);
            return await _userRepository.Update(mappedUser);
        }
    }
}
