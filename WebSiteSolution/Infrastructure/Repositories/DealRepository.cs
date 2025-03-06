using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DealRepository(Application.AppContext context) : IDealRepository
    {
        public Task<Deal?> GetById(int id)
        {
            var deal = context.Deals.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(deal);
        }

        public Task<IEnumerable<Deal>> GetAll()
        {
            return Task.FromResult<IEnumerable<Deal>>(context.Deals);
        }

        public Task Create(Deal deal)
        {
            context.Deals.Add(deal);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Update(Deal deal)
        {
            context.Deals.Update(deal);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(Deal deal)
        {
            context.Deals.Remove(deal);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
