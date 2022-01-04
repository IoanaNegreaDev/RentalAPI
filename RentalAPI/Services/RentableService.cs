using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentableService: BaseService<Vehicle, IRentableRepository>, IRentableService
    {
        private ICategoryRepository _categoryRepository;

        public RentableService(IRentableRepository repository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
          :base (repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Vehicle>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate)
        {
            var dbCategory = await _categoryRepository.FindByIdAsync(categoryId);
            if (dbCategory == null)
                return null;

            var result = await _repository.ListAvailableAsync(categoryId, startDate, endDate);
            return result;
        }
    }
}
