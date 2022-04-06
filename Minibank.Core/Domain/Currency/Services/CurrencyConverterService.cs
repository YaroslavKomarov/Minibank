using Minibank.Core.Domain.Exceptions;
using Minibank.Core.Domain.Currency;
using System.Threading;
using System;

namespace Minibank.Core.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private static readonly ValidCurrencies mainCurrencyCode = ValidCurrencies.RUB;

        private readonly ICurrencyRateService currencyRate;

        public CurrencyConverterService(ICurrencyRateService currencyRate)
        {
            this.currencyRate = currencyRate;
        }
  
        public decimal Convert(
            decimal? amount,
            string fromCurrency,
            string toCurrency, 
            CancellationToken cancellationToken)
        {
            ValidateArguments(amount, fromCurrency, toCurrency);
            var validAmount = (decimal)amount;

            if (fromCurrency == toCurrency)
            {
                return Math.Round(validAmount, 2);
            }
            else if (fromCurrency == mainCurrencyCode.ToString() && toCurrency != mainCurrencyCode.ToString())
            {
                return ConvertFromMainCurrency(validAmount, toCurrency, cancellationToken);
            }
            else if (fromCurrency != mainCurrencyCode.ToString() && toCurrency != mainCurrencyCode.ToString())
            {
                return ConvertBothNonMainCurrencies(validAmount, fromCurrency, toCurrency, cancellationToken);
            }
            else
            {
                return ConvertToMainCurrency(validAmount, fromCurrency, cancellationToken);
            }
        }

        private decimal ConvertFromMainCurrency(
            decimal amount, 
            string toCurrency, 
            CancellationToken cancellationToken)
        {
            var currencyAmount = amount / currencyRate.GetCurrencyRate(toCurrency, cancellationToken);
            return Math.Round(currencyAmount, 2);
        }

        private decimal ConvertBothNonMainCurrencies(
            decimal amount, 
            string fromCurrency, 
            string toCurrency, 
            CancellationToken cancellationToken)
        {
            var mainCurrencyAmount = amount * currencyRate.GetCurrencyRate(fromCurrency, cancellationToken);
            return ConvertFromMainCurrency(mainCurrencyAmount, toCurrency, cancellationToken);
        }

        private decimal ConvertToMainCurrency(
            decimal amount, 
            string fromCurrency, 
            CancellationToken cancellationToken)
        {
            var currencyAmount = amount * currencyRate.GetCurrencyRate(fromCurrency, cancellationToken);
            return Math.Round(currencyAmount, 2);
        }

        private static void ValidateArguments(decimal? amount, string fromCurrency, string toCurrency)
        {
            if (amount == null || amount < 0
                || string.IsNullOrWhiteSpace(toCurrency)
                || string.IsNullOrWhiteSpace(fromCurrency))
            {
                throw new ValidationException("Сумма недействительна или валютный(ые) код(ы) пуст(ы)");
            }
        }
    }
}
