using RentalAPI.Models;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IContractService:IBasicService<Contract>
    {
        public Task<Contract> FindByIdAsync(string userName, int contractId);
        public Task<DbOperationResponse<IEnumerable<Contract>>> ListAsync(string userId);
        public Task<DbOperationResponse<Contract>> AddAsync(string userId, int paymentCurrencyId);
        public Task<DbOperationResponse<Contract>> UpdateAsync(int contractId, int paymentCurrencyId);
        public Task<DbOperationResponse<Contract>> UpdateAsync(string userId, int contractId, int paymentCurrencyId);
    }
}
