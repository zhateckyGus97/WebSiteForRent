using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.DealRequests;
using Application.Responses;
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

        public async Task<int> Add(CreateDealRequest deal)
        {
            var deals = await _userRepository.GetAll();
            if (deals.Select(x => x.Id == deal.Id).ToList().Count() > 0)
            {
                throw new KeyAlreadyExistsException($"Id with value {deal.Id} already exists!");
            }

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

            await _dealRepository.Create(mappedDeal);
            return mappedDeal.Id;
        }

        public async Task<bool> Delete(int id)
        {
            return await _dealRepository.Delete(id);
        }

        public async Task<IEnumerable<DealResponse>> GetAll()
        {
            var deals = await _dealRepository.GetAll();
            var mappedDeals = deals.Select(d => _mapper.Map<DealResponse>(d)).ToList();
            return mappedDeals;
        }

        public async Task<DealResponse> GetById(int id)
        {
            var deal = await _dealRepository.GetById(id);
            if(deal == null)
            {
                throw new NotFoundApplicationException($"Deal with id {id} not found!");
            }
            
            return _mapper.Map<DealResponse>(deal);
        }

        public async Task<bool> Update(UpdateDealRequest deal)
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
