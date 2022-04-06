using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using Minibank.Core.Services;

namespace Minibank.Data.Currency.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly HttpClient httpClient;

        public CurrencyRateService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public decimal GetCurrencyRate(string currencyCode, CancellationToken cancellationToken)
        {
            var response = httpClient.GetFromJsonAsync<CurrencyRateResponse>("daily_json.js")
                 .GetAwaiter().GetResult();

            return response.Valute[currencyCode].Value;
        }
    }
}
