using Newtonsoft.Json.Linq;
using RentalAPI.Services.OperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RentalAPI.Services
{
    public class CurrencyRateExchanger: ICurrencyRateExchanger
    {
        private string _accessToken;
        private ICurrencyService _currencyService;

        public CurrencyRateExchanger(IConfiguration configuration, ICurrencyService currencyService)
        {          
            _accessToken = configuration.GetSection("ConnectionStrings:CurrencyLayerConnection").Value;
            _currencyService = currencyService;
        }

        private async Task<IRestResponse> GetCurrencyLayerExchangeRateAsync(string sourceCurrency,
                                                             string destinationCurrency)
        {
            try
            {
                var client = new RestClient($"http://apilayer.net/api/live" +
                                          $"?access_key=" + _accessToken +
                                          $"&currencies=" + destinationCurrency +
                                          $"&source=" + sourceCurrency +
                                          $"&format=1");

                var request = new RestRequest(Method.GET);
                return await client.ExecuteAsync(request);
            }
            catch(Exception ex)
            {
                IRestResponse response = new RestResponse();
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorMessage = ex.Message;
                response.ErrorException = ex;
                return response;
            }
        }

        private BasicOperationResponse<float> ExtractConversionRateFromResponse(IRestResponse restResponse, 
                                            string sourceCurrency,
                                            string destinationCurrency)
        {
            try
            {
                var jsonObject = JObject.Parse(restResponse.Content);
                var exchangeRate = jsonObject["quotes"][sourceCurrency + destinationCurrency].Value<float>();
                return new BasicOperationResponse<float>(exchangeRate);
            }
            catch(Exception ex)
            {
                return new BasicOperationResponse<float>("Failed to parse json object for currency rate. " + ex.Message);
            }
        }

        public async Task<BasicOperationResponse<float>> GetExchangeRate(int sourceCurrencyId, int destinationCurrencyId)
        {
            try
            {
                var sourceCurrency = await _currencyService.FindByIdAsync(sourceCurrencyId);
                if (sourceCurrency == null)
                    return new BasicOperationResponse<float>("Destination currency not found in database.");

                var destinationCurrency = await _currencyService.FindByIdAsync(destinationCurrencyId);
                if (destinationCurrency == null)
                    return new BasicOperationResponse<float>("Destination currency not found in database.");

                var response = await GetCurrencyLayerExchangeRateAsync(sourceCurrency.Name, destinationCurrency.Name);

                if (!response.IsSuccessful)
                    return new BasicOperationResponse<float>("Fail to get conversion rate from https://currencylayer.com/. Error: " + response.ErrorMessage);

                return ExtractConversionRateFromResponse(response, sourceCurrency.Name, destinationCurrency.Name);
            }
            catch(Exception ex)
            {
                return new BasicOperationResponse<float>("An internal exception occured. " + ex.Message);
            }

        }
    }
}
