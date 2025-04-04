using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.ApartmentRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IApartmentRepository apartRepository, IUserRepository userRepository, IMapper mapper)
        {
            _apartmentRepository = apartRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateApartmentRequest apartment)
        {
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            var user = _userRepository.GetById(apartment.UserId);

            return await _apartmentRepository.Create(mappedApartment);
        }

        public async Task<bool> Delete(int id)
        {
            return await _apartmentRepository.Delete(id);
        }

        public async Task<IEnumerable<ApartmentResponse>> GetAll()
        {
            var aparts = await _apartmentRepository.GetAll();
            var mappedAparts = aparts.Select(a => _mapper.Map<ApartmentResponse>(a)).ToList();
            return mappedAparts;
        }

        public async Task<ApartmentResponse> GetById(int id)
        {
            var apart = await _apartmentRepository.GetById(id);
            if (apart == null)
            {
                throw new NotFoundApplicationException($"Apartment with id {id} not found!");
            }
            
            return _mapper.Map<ApartmentResponse>(apart);
        }

        public async Task<bool> Update(UpdateApartmentRequest apartment)
        {
            if (apartment == null)
            {
                return false;
            }

            var mappedApartment = _mapper.Map<Apartment>(apartment);
            var user = _userRepository.GetById(apartment.UserId);

            return await _apartmentRepository.Update(mappedApartment);
        }
    }
}
