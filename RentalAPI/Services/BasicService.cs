using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class BasicService<T, TRepository> : IBasicService<T> 
        where T:class
        where TRepository : IGenericRepository<T>
    {
        protected readonly TRepository _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public BasicService(TRepository repository, IUnitOfWork unitOfWork)
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

        virtual public async Task<DbOperationResponse<T>> DeleteAsync(int id)
        {
            var item = await _repository.FindByIdAsync(id);

            if (item == null)
                return new DbOperationResponse<T>("Item not found.");

            try
            {
                _repository.Remove(item);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<T>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<T>($"An error occurred when deleting the item: {ex.Message}");
            }
        }
    }
}
