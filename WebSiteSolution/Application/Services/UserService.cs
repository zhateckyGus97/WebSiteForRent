using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> Add(UserDTO user)
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

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _userRepository.GetAll();
            var mappedUsers = users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
            return mappedUsers;
        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            var mappedUser = _mapper.Map<UserDTO>(user);
            return mappedUser;
        }

        public async Task<bool> Update(UserDTO user)
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
