using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface ICurrencyRateExchanger
    {
        public Task<BasicOperationResponse<float>> GetExchangeRate(int sourceCurrencyId, int destinationCurrencyId);
        public Task<BasicOperationResponse<float>> Convert(string sourceCurrency,
                                    string destinationCurrency,
                                    float amount);
        public Task<BasicOperationResponse<float>> ConvertFromDefaultCurrency(int destinationCurrencyId, float amount);
    }
}
