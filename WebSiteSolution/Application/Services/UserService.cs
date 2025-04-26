using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.UserRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAttachmentService _attachmentService;
        private readonly IMapper _mapper;
        private IPasswordHasher _hasher;

        public UserService(IUserRepository userRepository, IApartmentRepository apartmentRepository, IDealRepository dealRepository,
            IReviewRepository reviewRepository, IMapper mapper,
             IAttachmentService attachmentService,
            IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _dealRepository = dealRepository;
            _reviewRepository = reviewRepository;
            _attachmentService = attachmentService;
            _mapper = mapper;
            _hasher = hasher;
        }
     
        public async Task<int> Add(RegistrationUserRequest user)
        {
            var mappedUser = _mapper.Map<User>(user);
            mappedUser.PasswordHash = _hasher.HashPassword(user.Password);
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

            var mappedUser = _mapper.Map<UserResponse>(user);

            if (mappedUser.LogoAttachmentId.HasValue)
            {
                var attachmentUrl = await _attachmentService
                    .GetPublicLinkAsync(mappedUser.LogoAttachmentId.Value);
                mappedUser.LogoAttachmentUrl = attachmentUrl;
            }

            return mappedUser;
        }

        public async Task<bool> Update(UpdateUserRequest user)
        {
            if (user == null)
                throw new NotFoundApplicationException($"User not found!");

            var mappedUser = _mapper.Map<User>(user);
            mappedUser.PasswordHash = _hasher.HashPassword(user.Password);
            return await _userRepository.Update(mappedUser);
        }
    }
}
