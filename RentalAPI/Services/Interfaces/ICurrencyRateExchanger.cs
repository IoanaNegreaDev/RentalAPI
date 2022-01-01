using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface ICurrencyRateExchanger
    {
        public Task<float> Convert(string sourceCurrency,
                                    string destinationCurrency,
                                    float amount);
    }
}
