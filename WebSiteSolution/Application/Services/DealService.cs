using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;

        public DealService(IDealRepository dealRepository, IUserRepository userRepository, IApartmentRepository apartmentRepository, IMapper mapper)
        {
            _dealRepository = dealRepository;
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(DealDTO deal)
        {
            var mappedDeal = _mapper.Map<Deal>(deal);
            if (mappedDeal == null)
            {
                return -1;
            }

            var user = _userRepository.GetById(deal.UserId);
            if (user == null)
            {
                return -1;
            }

            var apartment = _apartmentRepository.GetById(deal.ApartmentId);
            if (apartment == null)
            {
                return -1;
            }

            return await _dealRepository.Create(mappedDeal);
        }

        public async Task<bool> Delete(int id)
        {
            return await _dealRepository.Delete(id);
        }

        public async Task<IEnumerable<DealDTO>> GetAll()
        {
            var deals = await _dealRepository.GetAll();
            var mappedDeals = deals.Select(d => _mapper.Map<DealDTO>(d)).ToList();
            return mappedDeals;
        }

        public async Task<DealDTO> GetById(int id)
        {
            var deal = await _dealRepository.GetById(id);
            var mappedDeal = _mapper.Map<DealDTO>(deal);
            return mappedDeal;
        }

        public async Task<bool> Update(DealDTO deal)
        {
            if (deal == null)
            {
                return false;
            }

            var mappedDeal = _mapper.Map<Deal>(deal);
            return await _dealRepository.Update(mappedDeal);
        }
    }
}
