using System.Threading;

namespace Minibank.Core.Services
{
    public interface ICurrencyRateService
    {
        public decimal GetCurrencyRate(string currencyCode, CancellationToken cancellationToken);
    }
}

