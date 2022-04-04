using Minibank.Core.Domains.BankAccounts;
using FluentValidation;

namespace Minibank.Core.Domain.BankAccounts.BankAccountValidators
{
    public class BankAccountValidator : AbstractValidator<BankAccount>
    {
        public BankAccountValidator()
        {
            RuleSet("Close", () =>
            {
                RuleFor(a => a.Amount)
                    .Must(amount => amount == 0)
                    .WithMessage("Банковский аккаунт должен иметь нулевой баланс для закрытия");
            });
        }

        protected override void EnsureInstanceNotNull(object instance)
        {
            if (instance == null)
                throw new ValidationException("Банковский аккаунт не существует");
        }
    }
}
