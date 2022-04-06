using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domains.Users;
using FluentValidation;
using System.Threading;

namespace Minibank.Core.Domain.Users.UserValidators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(u => u.Login).NotEmpty().WithMessage("Login не должен быть пустым");
                RuleFor(u => u.Email).NotEmpty().WithMessage("Email не должен быть пустым");

                RuleFor(u => u.Login.Length).LessThanOrEqualTo(29).WithMessage("Login не должен превышать 20 символов");
                RuleFor(u => u.Email.Length).LessThanOrEqualTo(256).WithMessage("Email не должен превышать 256 символов");
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
