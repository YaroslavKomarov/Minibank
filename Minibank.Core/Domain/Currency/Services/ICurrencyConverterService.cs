using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domain.Currency.Services
{
    public interface ICurrencyConverterService
    {
        public Task<decimal> Convert(
            decimal? amount, 
            string fromCurrency, 
            string toCurrency, 
            CancellationToken cancellationToken);
    }
}
