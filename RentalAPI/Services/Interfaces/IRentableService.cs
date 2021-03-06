using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IRentableService : IBaseService<Vehicle>
    {
        public Task<IEnumerable<Vehicle>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
