using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domain.Currency.Services
{
    public interface ICurrencyRateService
    {
        public Task<decimal> GetCurrencyRate(string currencyCode, CancellationToken cancellationToken);
    }
}

