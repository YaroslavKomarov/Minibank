using Minibank.Core.Domain.Exceptions;
using Minibank.Core.Domain.Currency;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Minibank.Core.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private static readonly ValidCurrencies mainCurrency = ValidCurrencies.RUB;

        private readonly ICurrencyRateService currencyRateService;

        public CurrencyConverterService(ICurrencyRateService currencyRateService)
        {
            this.currencyRateService = currencyRateService;
        }
  
        public async Task<decimal> Convert(
            decimal? amount,
            string fromCurrency,
            string toCurrency, 
            CancellationToken cancellationToken)
        {
            var validAmount = ValidateArguments(amount, fromCurrency, toCurrency);

            if (fromCurrency == toCurrency)
            {
                return Math.Round(validAmount, 2);
            }
            else if (fromCurrency == mainCurrency.ToString() && toCurrency != mainCurrency.ToString())
            {
                var rate = await currencyRateService.GetCurrencyRate(
                    toCurrency, cancellationToken);

                return Math.Round(validAmount / rate, 2);
            }
            else if (fromCurrency != mainCurrency.ToString() && toCurrency != mainCurrency.ToString())
            {
                var fromRate = await currencyRateService.GetCurrencyRate(
                    fromCurrency, cancellationToken);
                var toRate = await currencyRateService.GetCurrencyRate(
                    toCurrency, cancellationToken);

                return Math.Round((validAmount * fromRate) / toRate, 2);
            }
            else
            {
                var rate = await currencyRateService.GetCurrencyRate(
                    fromCurrency, cancellationToken);

                return Math.Round(validAmount * rate, 2);
            }
        }

        private static decimal ValidateArguments(decimal? amount, string fromCurrency, string toCurrency)
        {
            if (amount == null || amount < 0
                || string.IsNullOrWhiteSpace(toCurrency)
                || string.IsNullOrWhiteSpace(fromCurrency))
            {
                throw new ValidationException("Сумма недействительна или валютный(ые) код(ы) пуст(ы)");
            }

            return (decimal)amount;
        }
    }
}
