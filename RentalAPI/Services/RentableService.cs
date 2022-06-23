using RentalAPI.Controllers.ResourceParameters;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentableService: BasicService<Rentable, IRentableRepository>, IRentableService
    {
        private ICategoryRepository _categoryRepository;

        public RentableService(IRentableRepository repository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
          :base (repository, unitOfWork)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<DbOperationResponse<PagedList<Rentable>>> ListAsync(RentablesResourceParameters rentablesResourceParameters)
        {
            var result = await _repository.ListAsync(rentablesResourceParameters);

            return new DbOperationResponse<PagedList<Rentable>>(result);
        }

        public async Task<DbOperationResponse<IEnumerable<Rentable>>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate)
        {
            var dbCategory = await _categoryRepository.FindByIdAsync(categoryId);
            if (dbCategory == null)
                return new DbOperationResponse<IEnumerable<Rentable>>("categoryId not fount in the database.");

            var result = await _repository.ListAvailableAsync(categoryId, startDate, endDate);

            return new DbOperationResponse<IEnumerable<Rentable>>(result);
        }
    }
}
