using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppartmentTypeRepository/*(Application.AppContext context)*/ : IAppartmentTypeRepository
    {
        public readonly DbSet<AppartmentType> appTypes;
        public Task Create(AppartmentType appType)
        {
            appTypes.Add(appType);
            /*context.AppartmentTypes.Add(appType);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task Delete(AppartmentType appType)
        {
            appTypes.Remove(appType);
            /*context.AppartmentTypes.Remove(appType);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }

        public Task<IEnumerable<AppartmentType>> GetAll()
        {
            return Task.FromResult<IEnumerable<AppartmentType>>(appTypes);
        }

        public Task<AppartmentType?> GetById(int id)
        {
            var appType = appTypes.FirstOrDefault(x => x.TypeId == id);
            //var appType = context.AppartmentTypes.FirstOrDefault(x => x.TypeId == id);
            return Task.FromResult(appType);
        }

        public Task Update(AppartmentType appType)
        {
            appTypes.Update(appType);
            /*context.AppartmentTypes.Update(appType);
            context.SaveChanges();*/
            return Task.CompletedTask;
        }
    }
}
