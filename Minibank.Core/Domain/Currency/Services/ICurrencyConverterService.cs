using System.Threading;

namespace Minibank.Core.Services
{
    public interface ICurrencyConverterService
    {
        public decimal Convert(
            decimal? amount, 
            string fromCurrency, 
            string toCurrency, 
            CancellationToken cancellationToken);
    }
}
