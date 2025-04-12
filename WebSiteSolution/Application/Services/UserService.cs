using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.UserRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Domain.Enums;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IApartmentRepository apartmentRepository, IDealRepository dealRepository,
            IReviewRepository reviewRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _dealRepository = dealRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        private string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password, 
                BCrypt.Net.BCrypt.GenerateSalt(12));

            return hash;
        }

        private bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public async Task<int> Add(RegistrationUserRequest user)
        {
            var mappedUser = _mapper.Map<User>(user);
            mappedUser.PasswordHash = HashPassword(user.Password);
            mappedUser.Role = UserRoles.User;

            return await _userRepository.Create(mappedUser);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new NotFoundApplicationException($"User with id {id} not found!");

            await _apartmentRepository.DeleteByOwnerId(id);
            await _reviewRepository.DeleteByUserId(id);
            await _dealRepository.DeleteByUserId(id);

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
            if (user == null)
                throw new NotFoundApplicationException($"User not found!");

            var mappedUser = _mapper.Map<User>(user);
            mappedUser.PasswordHash = HashPassword(user.Password);
            return await _userRepository.Update(mappedUser);
        }
    }
}
