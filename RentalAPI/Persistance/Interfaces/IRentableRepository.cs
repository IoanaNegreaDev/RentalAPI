using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IRentableRepository:IGenericRepository<Rentable>
    {
        public Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
