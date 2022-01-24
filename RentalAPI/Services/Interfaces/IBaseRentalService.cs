using RentalAPI.Services.OperationStatusEncapsulators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IBaseRentalService<T> where T : class
    {
        Task<DbOperationResponse<IEnumerable<T>>> ListAsync(int contractId);
        Task<DbOperationResponse<T>> FindByIdAsync(int contractId, int id);
        public Task<DbOperationResponse<T>> AddAsync(int contractId, T rental);
        public Task<DbOperationResponse<T>> UpdateAsync(int contractId, T rental);
        public Task<DbOperationResponse<T>> DeleteAsync(int contractId, int rentalId);
    }
}
