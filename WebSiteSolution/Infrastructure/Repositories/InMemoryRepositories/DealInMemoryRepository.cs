using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.InMemoryRepositories
{
    [ExcludeFromCodeCoverage]
    public class DealInMemoryRepository : IDealRepository
    {
        private readonly List<Deal> _deals;

        public DealInMemoryRepository()
        {
            _deals = new List<Deal>
            {
                new Deal{Id = 1, UserId = 1, ApartmentId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now, CreatedAt = DateTime.Now, TotalPrice = 2500 },
                new Deal{Id = 2, UserId = 2, ApartmentId = 2, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now, CreatedAt = DateTime.Now, TotalPrice = 12500 },
                new Deal{Id = 3, UserId = 3, ApartmentId = 3, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now, CreatedAt = DateTime.Now, TotalPrice = 7500 },
            };
        }

        public Task<Deal?> GetById(int id)
        {
            var deal = _deals.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(deal);
        }

        public Task<IEnumerable<Deal>> GetAll()
        {
            return Task.FromResult(_deals.AsEnumerable());
        }

        public Task<int> Create(Deal deal)
        {
            if (deal == null)
                throw new ArgumentNullException(nameof(deal));

            _deals.Add(deal);
            return Task.FromResult(deal.Id);
        }

        public Task<bool> Update(Deal deal)
        {
            if (deal == null)
                throw new ArgumentNullException(nameof(deal));

            var old_deal = _deals.FirstOrDefault(d => d.Id == deal.Id);

            if (old_deal == null)
                return Task.FromResult(false);

            old_deal.ApartmentId = deal.ApartmentId;
            old_deal.UserId = deal.UserId;
            old_deal.CheckInDate = deal.CheckInDate;
            old_deal.CheckOutDate = deal.CheckOutDate;
            old_deal.TotalPrice = deal.TotalPrice;
            old_deal.UpdatedAt = DateTime.Now;

            return Task.FromResult(true);
        }

        public Task<bool> Delete(int Id)
        {
            var deal = _deals.FirstOrDefault(x => x.Id == Id);

            if (deal == null)
                return Task.FromResult(false);

            _deals.Remove(deal);
            return Task.FromResult(true);
        }

        public Task DeleteByUserId(int user_id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByApartmentId(int apartment_id)
        {
            throw new NotImplementedException();
        }
    }
}
