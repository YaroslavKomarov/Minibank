using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyConverter
    {
        public decimal Convert(decimal? amount, string fromCurrency, string toCurrency);
    }
}
