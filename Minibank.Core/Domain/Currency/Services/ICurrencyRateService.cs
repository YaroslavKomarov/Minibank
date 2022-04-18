using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Services
{
    public interface ICurrencyRateService
    {
        public Task<decimal> GetCurrencyRate(string currencyCode, CancellationToken cancellationToken);
    }
}

