using System.Net.Http;
using System.Net.Http.Json;
using Minibank.Core.Services;
using Minibank.Data.HttpClients.Models;

namespace Minibank.Data.Services
{
    public class CurrencyRate : ICurrencyRate
    {
        private readonly HttpClient httpClient;

        public CurrencyRate(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public decimal GetCurrencyRate(string currencyCode)
        {
            var response = httpClient.GetFromJsonAsync<RateResponse>("daily_json.js")
                 .GetAwaiter().GetResult();

            return response.Valute[currencyCode].Value;
        }
    }
}
