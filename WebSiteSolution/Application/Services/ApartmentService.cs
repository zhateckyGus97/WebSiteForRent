using Application.Exceptions;
using Application.Interfaces;
using Application.Requests.ApartmentRequests;
using Application.Responses;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IApartmentService> _logger;

        public ApartmentService(IApartmentRepository apartRepository, IReviewRepository reviewRepository, 
            IDealRepository dealRepository, IMapper mapper, ILogger<IApartmentService> logger)
        {
            _apartmentRepository = apartRepository;
            _reviewRepository = reviewRepository;
            _dealRepository = dealRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Add(CreateApartmentRequest apartment)
        {
            var mappedApartment = _mapper.Map<Apartment>(apartment);
            var result = await _apartmentRepository.Create(mappedApartment);
            _logger.LogInformation($"Apartment was created with id: {result}");
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var apartment = await _apartmentRepository.GetById(id);
            if (apartment == null)
                throw new NotFoundApplicationException($"Apartment with id {id} not found!");

            await _reviewRepository.DeleteByApartmentId(id);
            await _dealRepository.DeleteByApartmentId(id);

            _logger.LogInformation($"Apartment with id: {id} was deleted");
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
            var mappedApartment = _mapper.Map<Apartment>(apartment);

            if (!await _apartmentRepository.Update(mappedApartment))
                throw new EntityUpdateException("Apartment wasn't updated!");

            _logger.LogInformation($"Apartment with id: {apartment.Id} was updated");
            return true;
        }
    }
}
