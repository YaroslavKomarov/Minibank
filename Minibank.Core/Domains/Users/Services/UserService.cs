using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using System;
using System.ComponentModel.DataAnnotations;

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

        public void PostUser(User user)
        {
            userRepository.PostUser(user);
        }

        public void DeleteUserById(string id)
        {
            var account = accountRepository.GetBankAccountByUserId(id);

            if (account != null)
            {
                throw new ValidationException("Невозможно удалить пользователя с открытым аккаунтом");
            }

            userRepository.DeleteUserById(id);
        }

        public void PutUser(User user)
        {
            if (!userRepository.PutUser(user))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }
        }
    }
}
