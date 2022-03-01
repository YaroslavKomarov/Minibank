using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyConverter
    {
        public decimal ConvertRubles(int amount, string currencyCode);
    }
}
