using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class ContractService: 
        BaseService<Contract, IContractRepository>,  
        IContractService
    {
        private ICurrencyRateExchanger _currencyExchanger;
        private ICurrencyRepository _currencyRepository;
        public ContractService(IContractRepository repository, 
                                      IUnitOfWork unitOfWork, 
                                      ICurrencyRateExchanger currencyExchanger,
                                      ICurrencyRepository currencyRepository)
            :base(repository, unitOfWork)
        {
            _currencyExchanger = currencyExchanger;
            _currencyRepository = currencyRepository;
        }

        override public async Task<DbOperationResponse<Contract>> AddAsync(Contract item)
        {
            try
            {
                var defaultCurrency = _currencyRepository.GetDefaultAsync();
                var exchangeRateResult = await _currencyExchanger.GetExchangeRate(defaultCurrency.Result.Id,
                                                                                  item.PaymentCurrencyId);
                if (!exchangeRateResult.Success)
                    return new DbOperationResponse<Contract>("Failed to get exchange rate.");

                item.ExchangeRate = exchangeRateResult._entity;
                item.CreationDate = DateTime.Today;

                await _repository.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<Contract>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Contract>("Failed to add " + typeof(Contract).ToString() + " to the database " + ex.Message);
            }
        }

        public async Task<DbOperationResponse<Contract>> UpdatePricesAsync(int contractId)
        {
            var dbContract = await FindByIdAsync(contractId);
            if (dbContract == null)
                return new DbOperationResponse<Contract>("Contract not found.");

           /* var vehicleRentals = dbContract.Rentals.Cast<VehicleRental>();

            var basePriceInDefaultCurrency = vehicleRentals.Sum(item => item.BasePrice);
            var damagePriceInDefaultCurrency = vehicleRentals.Sum(item => item.DamagePrice);
            var fullTankPriceInDefaultCurrency = (float)vehicleRentals.Where(item => item.FullTank == false)
                                                               .Sum(item => item.FullTankPrice);

            var baseConversionResult = await _currencyExchanger.ConvertFromDefaultCurrency(dbContract.PaymentCurrencyId,
                                                                     basePriceInDefaultCurrency);
            if (!baseConversionResult.Success)
                return new DbOperationResponse<VehicleContract>("Failed to convert base price.");

            var damageConversionResult = await _currencyExchanger.ConvertFromDefaultCurrency(dbContract.PaymentCurrencyId,
                                                                     damagePriceInDefaultCurrency);
            if (!damageConversionResult.Success)
                return new DbOperationResponse<VehicleContract>("Failed to convert damage price.");

            var fullTankConversionResult = await _currencyExchanger.ConvertFromDefaultCurrency(dbContract.PaymentCurrencyId,
                                                                      fullTankPriceInDefaultCurrency);
            if (!fullTankConversionResult.Success)
                return new DbOperationResponse<VehicleContract>("Failed to convert full tank price.");

            dbContract.TotalBasePriceInPaymentCurrency = baseConversionResult._entity;
            dbContract.TotalDamagePriceInPaymentCurrency = damageConversionResult._entity;
            dbContract.TotalDamagePriceInPaymentCurrency = fullTankConversionResult._entity;*/

            return new DbOperationResponse<Contract>(dbContract);
        }

        override public async Task<DbOperationResponse<Contract>> UpdateAsync(Contract contract)
        {
            var existingContract = await _repository.FindByIdAsync(contract.Id);

            if (existingContract == null)
                return new DbOperationResponse<Contract>("Contract not found.");

            try
            {
                _repository.Update(existingContract);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<Contract>(existingContract);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new DbOperationResponse<Contract>($"An error occurred when updating the contract: {ex.Message}");
            }
        }
    }
}
