using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IBaseService<T> where T : class      
    {
        public Task<IEnumerable<T>> ListAsync();
        public Task<DbOperationResponse<T>> AddAsync(T item);
        public Task<T> FindByIdAsync(int id);

        public Task<DbOperationResponse<T>> UpdateAsync(T item);
    }
}
