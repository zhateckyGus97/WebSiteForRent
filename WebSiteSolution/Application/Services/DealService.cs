using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.DealRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IDealService> _logger;

        public DealService(IDealRepository dealRepository, IMapper mapper, ILogger<IDealService> logger)
        {
            _dealRepository = dealRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateDealRequest deal)
        {
            var mappedDeal = _mapper.Map<Deal>(deal);
            var result = await _dealRepository.Create(mappedDeal);
            _logger.LogInformation($"Deal was created with id: {result}");
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var deal = await _dealRepository.GetById(id);
            if (deal == null)
                throw new NotFoundApplicationException($"Deal with id {id} not found!");

            _logger.LogInformation($"Deal with id: {id} was deleted");
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
            var mappedDeal = _mapper.Map<Deal>(deal);

            if (!await _dealRepository.Update(mappedDeal))
                throw new EntityUpdateException("Deal wasn't updated!");

            _logger.LogInformation($"Deal with id: {deal.Id} was updated");
            return true;
        }
    }
}
