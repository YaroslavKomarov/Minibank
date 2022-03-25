using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Services;

namespace Minibank.Core.Domains.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        private readonly IBankAccountRepository accountRepository;

        public UserService(IUserRepository userRepository, IBankAccountRepository accountRepository)
        {
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
        }

        public string CreateUser(User user)
        {
            return userRepository.CreateUser(user);
        }

        public void DeleteUserById(string id)
        {
            if (accountRepository.ExistBankAccountByUserId(id))
            {
                throw new ValidationException("Невозможно удалить пользователя с открытым аккаунтом");
            }

            if (!userRepository.DeleteUserById(id))
            {
                throw new ValidationException("Не удалось удалить пользователя");
            }
        }

        public void UpdateUser(User user)
        {
            if (!userRepository.UpdateUser(user))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }
        }
    }
}
