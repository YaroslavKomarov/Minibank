using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users;
using FluentValidation;

namespace Minibank.Core.Domain.Users.UserValidators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IUserRepository userRepository, IBankAccountRepository accountRepository)
        {
            RuleSet("Create", () =>
            {
                RuleFor(u => u.Login).NotNull().NotEmpty().WithMessage("Login не должен быть пустым")
                    .Must(login => string.IsNullOrEmpty(login) || login.Length <= 256).WithMessage("Login не должен превышать 256 символов");

                RuleFor(u => u.Email).NotNull().NotEmpty().WithMessage("Email не должен быть пустым")
                    .Must(email => string.IsNullOrEmpty(email) || email.Length <= 256).WithMessage("Email не должен превышать 256 символов");

                RuleFor(u => u.Login)
                    .MustAsync((login, token) => userRepository.CheckIsLoginUniqueAsync(login, token))
                    .WithMessage("Пользователь с таким логином уже зарегистрирован");

                RuleFor(u => u.Email)
                    .MustAsync((email, token) => userRepository.CheckIsEmailUniqueAsync(email, token))
                    .WithMessage("Пользователь с такой почтой уже зарегистрирован");
            });

            RuleSet("Delete", () =>
            {
                RuleFor(u => u.Id)
                    .MustAsync((id, token) => accountRepository.CheckDoesNotBankAccountExistByUserIdAsync(id, token))
                    .WithMessage("Невозможно удалить пользователя с открытым аккаунтом");
            });
        }

        protected override void EnsureInstanceNotNull(object instance)
        {
            if (instance == null)
            {
                throw new ValidationException("Пользователь не существует");
            }
        }
    }
}
