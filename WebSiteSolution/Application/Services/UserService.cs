using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.UserRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IUserService> _logger;

        public UserService(IUserRepository userRepository, IApartmentRepository apartmentRepository, IDealRepository dealRepository,
            IReviewRepository reviewRepository, IMapper mapper, ILogger<IUserService> logger)
        {
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _dealRepository = dealRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateUserRequest user)
        {
            var mappedUser = _mapper.Map<User>(user);
            var result = await _userRepository.Create(mappedUser);
            _logger.LogInformation($"User was created with id: {result}");
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                throw new NotFoundApplicationException($"User with id {id} not found!");

            await _apartmentRepository.DeleteByOwnerId(id);
            await _reviewRepository.DeleteByUserId(id);
            await _dealRepository.DeleteByUserId(id);

            _logger.LogInformation($"User with id: {id} was deleted");
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

            if (!await _userRepository.Update(mappedUser))
                throw new EntityUpdateException("User wasn't updated!");

            _logger.LogInformation($"User with id: {user.Id} was updated");

            return true;
        }
    }
}