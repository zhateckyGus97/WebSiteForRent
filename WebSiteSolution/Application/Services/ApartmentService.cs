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
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IApartmentRepository apartRepository, IMapper mapper)
        {
            _apartmentRepository = apartRepository;
            _mapper = mapper;
        }

        public async Task<int> Add(ApartmentDTO apartment)
        {
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            if (mappedApartment != null )
            {
                await _apartmentRepository.Create(mappedApartment);
                return mappedApartment.Id;
            }

            return -1;
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
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            return await _apartmentRepository.Update(mappedApartment);
        }
    }
}
