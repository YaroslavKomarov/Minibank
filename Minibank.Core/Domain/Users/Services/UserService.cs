﻿using ValidationException = Minibank.Core.Domain.Exceptions.ValidationException;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domain.Exceptions;
using System.Threading.Tasks;
using FluentValidation;
using System.Threading;

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

        public async Task<string> CreateUserAsync(User user, CancellationToken cancellationToken)
        {
            userValidator.ValidateAndThrow(user, options => options.IncludeRuleSets("Create"));

            if (!await userRepository.CheckIsLoginUniqueAsync(user.Login, cancellationToken))
            {
                throw new ValidationException("Пользователь с таким логином уже зарегистрирован");
            }

            if (!await userRepository.CheckIsEmailUniqueAsync(user.Email, cancellationToken))
            {
                throw new ValidationException("Пользователь с такой почтой уже зарегистрирован");
            }

            var userId = await userRepository.CreateUserAsync(user, cancellationToken);
            unitOfWork.SaveChanges();
            return userId;
        }

        public async Task DeleteUserByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (await accountRepository.CheckIsBankAccountByUserIdExistAsync(id, cancellationToken))
            {
                throw new ValidationException("Невозможно удалить пользователя с открытым аккаунтом");
            }

            if (!await userRepository.DeleteUserByIdAsync(id, cancellationToken))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            unitOfWork.SaveChanges();
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            if (!(await userRepository.UpdateUserAsync(user, cancellationToken)))
            {
                throw new ValidationException("Пользователь с переданным идентификатором не существует");
            }

            unitOfWork.SaveChanges();
        }
    }
}
