using ValidationException = Minibank.Core.Domain.Exceptions.ValidationException;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System.Threading.Tasks;
using FluentValidation;
using Minibank.Core.Domain.Exceptions;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IUserRepository userRepository;

        private readonly IValidator<User> userValidator;

        private readonly IBankAccountRepository accountRepository;

        public UserService(
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IValidator<User> userValidator,
            IBankAccountRepository accountRepository)
        {
            this.unitOfWork = unitOfWork;
            this.userRepository = userRepository;
            this.userValidator = userValidator;
            this.accountRepository = accountRepository;
        }

        public async Task<string> CreateUserAsync(User user)
        {
            userValidator.ValidateAndThrow(user, options => options.IncludeRuleSets("Create"));

            var userId = await userRepository.CreateUserAsync(user);
            unitOfWork.SaveChanges();
            return userId;
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            if (await accountRepository.CheckIsBankAccountByUserIdExistAsync(id))
            {
                throw new ValidationException("Невозможно удалить пользователя с открытым аккаунтом");
            }

            if (!await userRepository.DeleteUserByIdAsync(id))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            unitOfWork.SaveChanges();
        }

        public async Task UpdateUserAsync(User user)
        {
            if (!(await userRepository.UpdateUserAsync(user)))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            unitOfWork.SaveChanges();
        }
    }
}
