using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class AppartmentRepository(Application.AppContext context) : IAppartmentRepository
    {
        public Task Create(Appartment appartment)
        {
            context.Appartments.Add(appartment);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(Appartment appartment)
        {
            context.Appartments.Remove(appartment);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Appartment>> GetAll()
        {
            return Task.FromResult<IEnumerable<Appartment>>(context.Appartments);
        }

        public Task<Appartment?> GetById(int id)
        {
            var appartment = context.Appartments.FirstOrDefault(x => x.Id == id);
            return Task.FromResult(appartment);
        }

        public Task Update(Appartment appartment)
        {
            context.Appartments.Update(appartment);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
