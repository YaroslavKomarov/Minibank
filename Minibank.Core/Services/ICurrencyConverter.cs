using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyConverter
    {
        public decimal ConvertRubles(decimal? amount, string currencyCode);
    }
}
