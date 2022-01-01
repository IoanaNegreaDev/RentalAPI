using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class CurrencyService:ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(ICurrencyRepository currencyRepository,
                                   IUnitOfWork unitOfWork)
        {
            this._currencyRepository = currencyRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Currency>> ListAsync()
        {
            return await _currencyRepository.ListAsync();
        }

        public async Task<Currency> FindByNameAsync(string name)
        {
            return await _currencyRepository.FindByNameAsync(name);
        }

        public async Task<Currency> GetDefaultAsync()
        {
            return await _currencyRepository.GetDefaultAsync();
        }
    }
}
