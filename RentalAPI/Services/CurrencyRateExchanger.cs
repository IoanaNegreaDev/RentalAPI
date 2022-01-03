using Newtonsoft.Json.Linq;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class CurrencyRateExchanger: ICurrencyRateExchanger
    {
        private string _accessToken;
        private ICurrencyService _currencyService;

        public CurrencyRateExchanger(ICurrencyService currencyService)
        {
            _accessToken = "3e3c7750926ef2253fe526e47b87b16b";
            _currencyService = currencyService;
        }

        private async Task<IRestResponse> GetCurrencyLayerExchangeRateAsync(string sourceCurrency,
                                                                       string destinationCurrency)
        {
            var client = new RestClient($"http://apilayer.net/api/live" +
                                             $"?access_key=" + _accessToken +
                                             $"&currencies=" + destinationCurrency +
                                             $"&source=" + sourceCurrency +
                                             $"&format=1");

            var request = new RestRequest(Method.GET);
            return await client.ExecuteAsync(request);
        }

        private float ExtractConversionRateFromResponse(IRestResponse restResponse, 
                                            string sourceCurrency,
                                            string destinationCurrency)
        {
            var jsonObject = JObject.Parse(restResponse.Content);
            return jsonObject["quotes"][sourceCurrency + destinationCurrency].Value<float>();
        }

        public async Task<BasicOperationResponse<float>> Convert(string sourceCurrency,
                                                             string destinationCurrency,
                                                             float amount)
        {
            var response = await GetCurrencyLayerExchangeRateAsync(sourceCurrency, destinationCurrency);

            if (!response.IsSuccessful)
                return new BasicOperationResponse<float>("Fail to get conversion rate from https://currencylayer.com/. Error: " + response.ErrorMessage);
           
            var conversionRate = ExtractConversionRateFromResponse(response, sourceCurrency, destinationCurrency);

            return new BasicOperationResponse<float>(amount * conversionRate);
        }

        public async Task<BasicOperationResponse<float>> ConvertFromDefaultCurrency(int destinationCurrencyId, float amount)
        {
            var destinationCurrency = await _currencyService.FindByIdAsync(destinationCurrencyId);
            if (destinationCurrency == null)
                return new BasicOperationResponse<float>("Destination currency not found in database.");

            if (!destinationCurrency.Default)
            {
                var defaultCurrency = await _currencyService.GetDefaultAsync();
                return await Convert(defaultCurrency.Name, destinationCurrency.Name, amount);
            }

            return new BasicOperationResponse<float>(amount);
        }
    }
}
