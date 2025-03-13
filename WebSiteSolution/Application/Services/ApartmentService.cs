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
    public class ApartmentService : IApartmentService
    {
        private IApartmentRepository _apartRepository;
        private IMapper _mapper;

        public ApartmentService(IApartmentRepository apartRepository, IMapper mapper)
        {
            this._apartRepository = apartRepository;
            this._mapper = mapper;
        }

        public async Task Add(ApartmentDTO apartment)
        {
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            await _apartRepository.Create(mappedApartment);
        }

        public async Task<bool> Delete(int id)
        {
            return await _apartRepository.Delete(id);
        }

        public async Task<IEnumerable<ApartmentDTO>> GetAll()
        {
            var aparts = await _apartRepository.GetAll();
            var mappedAparts = aparts.Select(a => _mapper.Map<ApartmentDTO>(a)).ToList();
            return mappedAparts;
        }

        public async Task<ApartmentDTO> GetById(int id)
        {
            var apart = await _apartRepository.GetById(id);
            var mappedAparts = _mapper.Map<ApartmentDTO>(apart);
            return mappedAparts;
        }

        public Task<bool> Update(ApartmentDTO apartment)
        {
            throw new NotImplementedException();
        }
    }
}
