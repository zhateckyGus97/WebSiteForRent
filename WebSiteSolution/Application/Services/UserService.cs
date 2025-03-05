using Application.DTO;
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
    public class IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public IUserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var users = await _userRepository.ReadAll();
            var mappedUsers = users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
            return mappedUsers;
        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            var mappedUser = _mapper.Map<UserDTO>(user);
            return mappedUser;
        }
    }
}
