using Minibank.Core.Domain.Exceptions;
using System.Threading.Tasks;
using Minibank.Core.Services;
using System.Net.Http.Json;
using System.Threading;
using System.Net.Http;

namespace Minibank.Data.Currency.Services
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly HttpClient httpClient;

        public CurrencyRateService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<decimal> GetCurrencyRate(string currencyCode, CancellationToken cancellationToken)
        {
            try
            {
                var response = await httpClient
                    .GetFromJsonAsync<CurrencyRateResponse>("daily_json.js", cancellationToken);

                return response.Valute[currencyCode].Value;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                {
                    throw new ValidationException("Не удалось получить доступ к сервису, повторите попытку позже");
                }
                throw new ValidationException("Запршиваемый сервис недоступен");
            }
        }
    }
}
