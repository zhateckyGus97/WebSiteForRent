using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class InMemoryApartmentRepository : IApartmentRepository
    {
        private readonly List<Apartment> _apartments;

        public InMemoryApartmentRepository()
        {
            _apartments = new List<Apartment>()
            {
                new Apartment{Id = 1, Title = "Standart", Description = "Standart apartment", Address = "Yaroslavl", PricePerDay = 2500, NumOfFloor = 1, Square = 34, Capacity = 2 },
                new Apartment{Id = 2, Title = "Luxe", Description = "Luxe apartment", Address = "Yaroslavl", PricePerDay = 12500, NumOfFloor = 2, Square = 105, Capacity = 8 },
                new Apartment{Id = 3, Title = "Comfort", Description = "Comfort apartment", Address = "Yaroslavl", PricePerDay = 7500, NumOfFloor = 1, Square = 72, Capacity = 4 }
            };
        }

        public Task<int> Create(Apartment apartment)
        {
            if(apartment == null) 
                throw new ArgumentNullException(nameof(apartment));

            _apartments.Add(apartment);
            return Task.FromResult(apartment.Id);
        }

        public Task<bool> Delete(int Id)
        {
            var apart = _apartments.FirstOrDefault(a => a.Id == Id);
            if(apart == null)
                return Task.FromResult(false);

            _apartments.Remove(apart);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<Apartment>> GetAll()
        {
            return Task.FromResult(_apartments.AsEnumerable());
        }

        public Task<Apartment?> GetById(int id)
        {
            var apartment = _apartments.FirstOrDefault(a => a.Id == id);
            return Task.FromResult(apartment);
        }

        public Task<bool> Update(Apartment apartment)
        {
            if (apartment == null)
                throw new ArgumentNullException(nameof(apartment));

            var old_apart = _apartments.FirstOrDefault(a => a.Id == apartment.Id);

            if (old_apart == null)
                return Task.FromResult(false);

            old_apart.PricePerDay = apartment.PricePerDay;
            old_apart.Capacity = apartment.Capacity;
            old_apart.Title = apartment.Title;
            old_apart.Description = apartment.Description;
            old_apart.Address = apartment.Address;
            old_apart.NumOfFloor = apartment.NumOfFloor;
            old_apart.Square = apartment.Square;

            return Task.FromResult(true);
        }
    }
}
