using RentalAPI.Models;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IRentableService : IBasicService<Rentable>
    {
        public Task<DbOperationResponse<Rentable>> FindByIdAsync(int categoryId, int id);
        public Task<DbOperationResponse<IEnumerable<Rentable>>> ListAsync(int categoryId);
        public Task<DbOperationResponse<IEnumerable<Rentable>>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate);
    }
}
