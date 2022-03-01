using System.Collections.Generic;

namespace Minibank.Core.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyRate currencyRate;

        public CurrencyConverter(ICurrencyRate currencyRate)
        {
            this.currencyRate = currencyRate;
        }
  
        public int ConvertRubles(int amount, string currencyCode)
        {
            if (amount < 0 || string.IsNullOrWhiteSpace(currencyCode))
                throw new InvalidCurrencyArgsException("Amount is invalid or currency code is empty");
            else
                return (int)(amount / currencyRate.GetCurrencyRate(currencyCode));
        }
    }
}
