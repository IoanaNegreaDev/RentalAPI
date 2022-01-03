using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class BaseService<T, TRepository> : IBaseService<T> 
        where T:class
        where TRepository : IGenericRepository<T>
    {
        protected readonly TRepository _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(TRepository repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }
        virtual public async Task<IEnumerable<T>> ListAsync()
            => await _repository.ListAsync();
        virtual public async Task<DbOperationResponse<T>> AddAsync(T item)
        {
            try
            {
                await _repository.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<T>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<T>("Failed to add " + typeof(T).ToString() + " to the database " + ex.Message);
            }
        }

        virtual public async Task<T> FindByIdAsync(int id)
          => await _repository.FindByIdAsync(id);
        virtual public Task<DbOperationResponse<T>> UpdateAsync(T item)
            => throw new NotImplementedException();
    }
}
