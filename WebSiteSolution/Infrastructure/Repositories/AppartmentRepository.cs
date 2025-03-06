using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppartmentRepository/*(Application.AppContext context)*/ : IAppartmentRepository
    {
        public readonly DbSet<Appartment> appartments;
        public Task Create(Appartment appartment)
        {
            appartments.Add(appartment);
            /*context.Appartments.Add(appartment);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task Delete(Appartment appartment)
        {
            appartments.Remove(appartment);
            /*context.Appartments.Remove(appartment);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Appartment>> GetAll()
        {
            return Task.FromResult<IEnumerable<Appartment>>(appartments);
        }

        public Task<Appartment?> GetById(int id)
        {
            //var appartment = context.Appartments.FirstOrDefault(x => x.Id == id
            var appartment = appartments.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(appartment);
        }

        public Task Update(Appartment appartment)
        {
            appartments.Update(appartment);
            /*context.Appartments.Update(appartment);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }
    }
}
