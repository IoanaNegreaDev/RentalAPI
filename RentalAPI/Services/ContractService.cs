using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RentalAPI.Services
{
    public class ContractService: 
        BaseService<Contract, IContractRepository>,  
        IContractService
    {
        private ICurrencyRateExchanger _currencyExchanger;
        private ICurrencyRepository _currencyRepository;
        private IUserRepository _userRepository;
        public ContractService(IContractRepository repository, 
                                      IUnitOfWork unitOfWork, 
                                      ICurrencyRateExchanger currencyExchanger,
                                      ICurrencyRepository currencyRepository,
                                       IUserRepository userRepository

            )
            :base(repository, unitOfWork)
        {
            _currencyExchanger = currencyExchanger;
            _currencyRepository = currencyRepository;
            _userRepository = userRepository;
        }

        override public async Task<DbOperationResponse<Contract>> AddAsync(Contract item)
        {
            try
            {
                var dbCurrency = await _currencyRepository.FindByIdAsync(item.PaymentCurrencyId);
                if (dbCurrency == null)
                    return new DbOperationResponse<Contract>("Currency not supported. Check the supported currencies.");          
                          
                var defaultCurrency = await _currencyRepository.GetDefaultAsync();
                if (defaultCurrency == null)
                    return new DbOperationResponse<Contract>("Internal error. Failed to detect the application default currency.");

                var exchangeRateResult = await _currencyExchanger.GetExchangeRate(defaultCurrency.Id,
                                                                                  item.PaymentCurrencyId);
                if (!exchangeRateResult.Success)
                    return new DbOperationResponse<Contract>("Failed to get exchange rate.");

                var dbUser = await _userRepository.FindByIdAsync(item.User.UserName);
                if (dbUser != null)
                    item.User = dbUser;
                else
                    return new DbOperationResponse<Contract>("Failed to find user.");

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

        public async Task<DbOperationResponse<Contract>> AddAsync(string userName, int paymentCurrencyId)
        {
            try
            {
                var dbCurrency = await _currencyRepository.FindByIdAsync(paymentCurrencyId);
                if (dbCurrency == null)
                    return new DbOperationResponse<Contract>("Currency not supported. Check the supported currencies.");

                var defaultCurrency = await _currencyRepository.GetDefaultAsync();
                if (defaultCurrency == null)
                    return new DbOperationResponse<Contract>("Internal error. Failed to detect the application default currency.");

                var exchangeRateResult = await _currencyExchanger.GetExchangeRate(defaultCurrency.Id,
                                                                                  paymentCurrencyId);
                if (!exchangeRateResult.Success)
                    return new DbOperationResponse<Contract>("Failed to get exchange rate.");

                var dbUser = await _userRepository.FindByIdAsync(userName);
                if (dbUser == null)
                    return new DbOperationResponse<Contract>("Failed to find user.");

                var newContract = new Contract
                {
                    User = dbUser,
                    PaymentCurrencyId = paymentCurrencyId,
                    ExchangeRate = exchangeRateResult._entity,
                    CreationDate = DateTime.Today
                };

                await _repository.AddAsync(newContract);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<Contract>(newContract);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Contract>("Failed to add " + typeof(Contract).ToString() + " to the database " + ex.Message);
            }
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
                return new DbOperationResponse<Contract>($"An error occurred when updating the contract: {ex.Message}");
            }
        }
        
        override public async Task<DbOperationResponse<Contract>> DeleteAsync(int id)
        {
            var item = await _repository.FindByIdAsync(id);

            if (item == null)
                return new DbOperationResponse<Contract>("Item not found.");


            var hasRegisteredDamagesPerRental = item.Rentals
                                                    .Where(r => r != null && 
                                                           r.RentalDamages != null && 
                                                           r.RentalDamages.Count() > 0)
                                                    .Any();

            if (hasRegisteredDamagesPerRental)
                return new DbOperationResponse<Contract>("Deletion of contracts with reported damages is forbidden.");

            try
            {
                await _repository.RemoveAsync(item);
                await _unitOfWork.SaveChangesAsync();

                return new DbOperationResponse<Contract>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Contract>($"An error occurred when deleting the item: {ex.Message}");
            }
        }
    }
}
