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
    public class DealRepository/*(Application.AppContext context)*/ : IDealRepository
    {
        public readonly DbSet<Deal> Deals;
        public Task<Deal?> GetById(int id)
        {
            var deal = Deals.FirstOrDefault(x => x.Id == id);
            //var deal = context.Deals.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(deal);
        }

        public Task<IEnumerable<Deal>> GetAll()
        {
            return Task.FromResult<IEnumerable<Deal>>(Deals);
        }

        public Task Create(Deal deal)
        {
            Deals.Add(deal);
            /*context.Deals.Add(deal);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task Update(Deal deal)
        {
            Deals.Update(deal);
            /*context.Deals.Update(deal);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task Delete(Deal deal)
        {
            Deals.Remove(deal);
            /*context.Deals.Remove(deal);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }
    }
}
