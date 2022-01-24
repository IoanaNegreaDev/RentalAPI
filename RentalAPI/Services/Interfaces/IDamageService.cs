using RentalAPI.Models;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IDamageService:IBasicService<Damage>
    {
        public Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(int contractId, int rentalId);
        public Task<DbOperationResponse<Damage>> AddAsync(int contractId, int rentalId, Damage item);
        public Task<DbOperationResponse<Damage>> FindByIdAsync(int contractId, int rentalId, int damageId);
        public Task<DbOperationResponse<Damage>> UpdateAsync(int contractId, int rentalId, Damage damage);
        public Task<DbOperationResponse<Damage>> DeleteAsync(int contractId, int rentalId, int damageId);
    }
}
