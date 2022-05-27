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
                .NotNull().NotEmpty().WithMessage("Валютный код источника пуст")
                .Must(currency => string.IsNullOrEmpty(currency) || Enum.IsDefined(typeof(ValidCurrencies), currency.ToUpperInvariant()))
                .WithMessage("Недопустимый валютный код источника");

            RuleFor(c => c.ToCurrency)
                .NotNull().NotEmpty().WithMessage("Валютный код назначения пуст")
                .Must(currency => string.IsNullOrEmpty(currency) || Enum.IsDefined(typeof(ValidCurrencies), currency.ToUpperInvariant()))
                .WithMessage("Недопустимый валютный код назначения");
        }
    }
}
