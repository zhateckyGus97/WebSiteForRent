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
        private IDealRepository _dealRepository;
        private IMapper _mapper;

        public DealService(IDealRepository dealRepository, IMapper mapper)
        {
            this._dealRepository = dealRepository;
            this._mapper = mapper;
        }

        public async Task Add(DealDTO deal)
        {
            var mappedDeal = _mapper.Map<Deal>(deal);
            await _dealRepository.Create(mappedDeal);
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

        public Task<bool> Update(DealDTO deal)
        {
            throw new NotImplementedException();
        }
    }
}
