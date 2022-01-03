using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentableService: BaseService<Vehicle, IRentableRepository>, IRentableService
    {
        public RentableService(IRentableRepository repository, IUnitOfWork unitOfWork)
          :base (repository, unitOfWork)
        {
        }

        public async Task<IEnumerable<Vehicle>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate)
            => await _repository.ListAvailableAsync(categoryId, startDate, endDate);
    }
}
