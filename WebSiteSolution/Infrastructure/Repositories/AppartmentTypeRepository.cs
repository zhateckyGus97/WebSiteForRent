using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AppartmentTypeRepository(Application.AppContext context) : IAppartmentTypeRepository
    {
        public Task Create(AppartmentType appType)
        {
            context.AppartmentTypes.Add(appType);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task Delete(AppartmentType appType)
        {
            context.AppartmentTypes.Remove(appType);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<AppartmentType>> GetAll()
        {
            return Task.FromResult<IEnumerable<AppartmentType>>(context.AppartmentTypes);
        }

        public Task<AppartmentType?> GetById(int id)
        {
            var appType = context.AppartmentTypes.FirstOrDefault(x => x.TypeId == id);
            return Task.FromResult(appType);
        }

        public Task Update(AppartmentType appType)
        {
            context.AppartmentTypes.Update(appType);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
