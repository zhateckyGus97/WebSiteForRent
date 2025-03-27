using Application.DTO;
using Application.Interfaces;
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

        public async Task<int> Add(ApartmentDTO apartment)
        {
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            if (mappedApartment == null)
            {
                return -1;
            }

            var user = _userRepository.GetById(apartment.UserId);
            if (user == null)
            {
                return -1;
            }

            return await _apartmentRepository.Create(mappedApartment);
        }

        public async Task<bool> Delete(int id)
        {
            return await _apartmentRepository.Delete(id);
        }

        public async Task<IEnumerable<ApartmentDTO>> GetAll()
        {
            var aparts = await _apartmentRepository.GetAll();
            var mappedAparts = aparts.Select(a => _mapper.Map<ApartmentDTO>(a)).ToList();
            return mappedAparts;
        }

        public async Task<ApartmentDTO> GetById(int id)
        {
            var apart = await _apartmentRepository.GetById(id);
            var mappedAparts = _mapper.Map<ApartmentDTO>(apart);
            return mappedAparts;
        }

        public async Task<bool> Update(ApartmentDTO apartment)
        {
            if (apartment == null)
            {
                return false;
            }

            var mappedApartment = _mapper.Map<Apartment>(apartment);

            var user = _userRepository.GetById(apartment.UserId);
            if (user == null)
            {
                return false;
            }

            return await _apartmentRepository.Update(mappedApartment);
        }
    }
}
