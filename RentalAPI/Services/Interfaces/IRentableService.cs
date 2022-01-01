using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IRentableService
    {
        public Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
        public Task<Rentable> FindByIdAsync(int id);
    }
}
