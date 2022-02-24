using System.Collections.Generic;

namespace Minibank.Core.Services
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public CurrencyConverter(ICurrencyRate currencyRate)
        {
            _currencyRate = currencyRate;
        }
  
        public int ConvertRubles(int amount, string currencyCode)
        {
            if (amount < 0 || CheckCodeIsValid(currencyCode))
                throw new UserFriendlyException("Friendly error message");
            else
                return (int)(amount / _currencyRate.GetCurrencyRate(currencyCode));
        }

        private static bool CheckCodeIsValid(string currencyCode)
        {
            return currencyCode == "" || string.IsNullOrWhiteSpace(currencyCode);
        }

        private readonly ICurrencyRate _currencyRate;
    }
}
