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
        private IClientRepository _clientRepository;
        public ContractService(IContractRepository repository, 
                                      IUnitOfWork unitOfWork, 
                                      ICurrencyRateExchanger currencyExchanger,
                                      ICurrencyRepository currencyRepository,
                                      IClientRepository clientRepository)
            :base(repository, unitOfWork)
        {
            _currencyExchanger = currencyExchanger;
            _currencyRepository = currencyRepository;
            _clientRepository = clientRepository;
        }

        override public async Task<DbOperationResponse<Contract>> AddAsync(Contract item)
        {
            try
            {
                var dbCurrency = await _currencyRepository.FindByIdAsync(item.PaymentCurrencyId);
                if (dbCurrency == null)
                    return new DbOperationResponse<Contract>("Currency not supported. Check https://localhost:5001/Currencies for supported currencies.");          

                var dbClient = await _clientRepository.FindByNameAsync(item.Client.Name);
                if (dbClient != null)
                {
                    item.ClientId = dbClient.Id;
                    item.Client = null;
                }

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
