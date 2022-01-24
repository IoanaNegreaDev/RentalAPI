using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IRentableRepository:IGenericRepository<Rentable>
    {
        public Task<Rentable> FindByIdAsync(int categoryId, int id);
        public Task<bool> IsAvailable(int id, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<Rentable>> ListAsync(int categoryId);
        public Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
