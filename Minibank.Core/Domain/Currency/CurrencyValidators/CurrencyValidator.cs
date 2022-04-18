using FluentValidation;
using System;

namespace Minibank.Core.Domain.Currency.CurrencyValidators
{
    public class CurrencyValidator : AbstractValidator<ConvertCurrencyDto>
    {
        public CurrencyValidator()
        {
            RuleFor(c => c.Amount)
                .Must(amount => amount.HasValue && amount >= 0)
                .WithMessage("Передана невалидная сумма");

            RuleFor(c => c.FromCurrency)
                .Must(currency => !string.IsNullOrWhiteSpace(currency))
                .WithMessage("Валютный код источника пуст");
            RuleFor(c => c.FromCurrency)
                .Must(currency => Enum.IsDefined(typeof(ValidCurrencies), currency.ToUpperInvariant()))
                .WithMessage("Недопустимый валютный код");

            RuleFor(c => c.ToCurrency)
                .Must(currency => !string.IsNullOrWhiteSpace(currency))
                .WithMessage("Валютный код назначения пуст");
            RuleFor(c => c.ToCurrency)
                .Must(currency => Enum.IsDefined(typeof(ValidCurrencies), currency.ToUpperInvariant()))
                .WithMessage("Недопустимый валютный код");
        }
    }
}
