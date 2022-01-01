using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentableService:IRentableService
    {
        private readonly IRentableRepository _rentablesRepository;

        public RentableService(IRentableRepository rentablesRepository)
        {
            this._rentablesRepository = rentablesRepository;
        }

        public async Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, DateTime startDate, DateTime endDate)
        {
            return await _rentablesRepository.ListAvailableAsync(categoryId, startDate, endDate);
        }

        public async Task<Rentable> FindByIdAsync(int id)
        {
            return await _rentablesRepository.FindByIdAsync(id);
        }
    }
}
