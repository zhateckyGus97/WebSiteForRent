using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public Task Add(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
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

        public Task Update(UserDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
