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
        private readonly IMapper _mapper;

        public DealService(IDealRepository dealRepository, IMapper mapper)
        {
            _dealRepository = dealRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateDealRequest deal)
        {
            var mappedDeal = _mapper.Map<Deal>(deal);
            return await _dealRepository.Create(mappedDeal);
        }

        public async Task<bool> Delete(int id)
        {
            var deal = await _dealRepository.GetById(id);
            if (deal == null)
                throw new NotFoundApplicationException($"Deal with id {id} not found!");

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
            if (deal == null)
            {
                throw new NotFoundApplicationException($"Deal with id {id} not found!");
            }
            
            return _mapper.Map<DealResponse>(deal);
        }

        public async Task<bool> Update(UpdateDealRequest deal)
        {
            if (deal == null)
                throw new NotFoundApplicationException($"Deal not found!");

            var mappedDeal = _mapper.Map<Deal>(deal);
            return await _dealRepository.Update(mappedDeal);
        }
    }
}
