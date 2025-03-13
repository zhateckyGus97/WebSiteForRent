using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DealRepository : IDealRepository
    {
        private readonly List<Deal> _deals = new List<Deal>();
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
            if(deal == null)
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
            var deal = _deals.FirstOrDefault(x =>x.Id == Id);

            if (deal == null)
                return Task.FromResult(false);

            _deals.Remove(deal);
            return Task.FromResult(true);
        }
    }
}
