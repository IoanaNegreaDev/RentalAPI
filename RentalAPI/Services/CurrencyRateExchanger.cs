using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using RentalAPI.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class CurrencyRateExchanger: ICurrencyRateExchanger
    {
        string _accessToken;
        public CurrencyRateExchanger()
        {
            _accessToken = "3e3c7750926ef2253fe526e47b87b16b";
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

        public async Task<float> Convert(string sourceCurrency,
                                         string destinationCurrency,
                                         float amount)
        {
            var response = await GetCurrencyLayerExchangeRateAsync(sourceCurrency, destinationCurrency);

            if (!response.IsSuccessful)
                throw new ArgumentException("Fail to get conversion rate from https://currencylayer.com/. Error: " + response.ErrorMessage);
           
            var conversionRate = ExtractConversionRateFromResponse(response, sourceCurrency, destinationCurrency);

            return amount * conversionRate;
        }
    }
}
