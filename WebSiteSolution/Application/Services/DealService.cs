using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> Add(DealDTO deal)
        {
            var mappedDeal = _mapper.Map<Deal>(deal);
            if (mappedDeal == null)
            {
                await _dealRepository.Create(mappedDeal);
                return mappedDeal.Id;
            }

            return -1;
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
            var mappedDeal = _mapper.Map<Deal>(deal);
            return await _dealRepository.Update(mappedDeal);
        }
    }
}
