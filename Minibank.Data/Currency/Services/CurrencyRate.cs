using System.Net.Http;
using System.Net.Http.Json;
using Minibank.Core.Services;

namespace Minibank.Data.Currency.Services
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
            var response = httpClient.GetFromJsonAsync<CurrencyRateResponse>("daily_json.js")
                 .GetAwaiter().GetResult();

            return response.Valute[currencyCode].Value;
        }
    }
}
