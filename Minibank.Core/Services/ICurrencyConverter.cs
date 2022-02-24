using System;

namespace Minibank.Core.Services
{
    public interface ICurrencyConverter
    {
        public int ConvertRubles(int amount, string currencyCode);
    }
}
