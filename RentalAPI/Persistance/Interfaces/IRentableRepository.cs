using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IRentableRepository:IGenericRepository<Vehicle>
    {
        public Task<bool> IsAvailable(int id, DateTime startDate, DateTime endDate);
        public Task<IEnumerable<Vehicle>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
