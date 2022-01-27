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
        Task<DbOperationResponse<Damage>> FindByIdAsync(int contractId, int rentalId, int damageId);
        Task<DbOperationResponse<Damage>> FindByIdAsync(string userId, int contractId, int rentalId, int damageId);

        Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(int contractId, int rentalId);
        Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(string userId, int contractId, int rentalId);

        Task<DbOperationResponse<Damage>> AddAsync(int contractId, int rentalId, Damage item);

        Task<DbOperationResponse<Damage>> UpdateAsync(int contractId, int rentalId, Damage damage);

        Task<DbOperationResponse<Damage>> DeleteAsync(int contractId, int rentalId, int damageId);
    }
}
