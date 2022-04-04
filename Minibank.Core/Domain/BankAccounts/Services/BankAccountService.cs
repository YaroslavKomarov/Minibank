using ValidationException = Minibank.Core.Domain.Exceptions.ValidationException;
using Minibank.Core.Domains.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.MoneyTransfersHistory;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domain.Currency;
using Minibank.Core.Domains.Users;
using System.Threading.Tasks;
using Minibank.Core.Services;
using FluentValidation;
using System;
using Minibank.Core.Domain.Exceptions;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    internal class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IValidator<User> userValidator;

        private readonly IValidator<BankAccount> bankAccountValidator;

        private readonly IBankAccountRepository accountRepository;

        private readonly IMoneyTransferHistoryRepository historyRepository;

        private readonly IUserRepository userRepository;

        private readonly ICurrencyConverter converter;

        private static readonly int commissionPercentage = 2;

        public BankAccountService(
            IUnitOfWork unitOfWork,
            IValidator<User> userValidator,
            IValidator<BankAccount> bankAccountValidator,
            IMoneyTransferHistoryRepository historyRepository,
            IBankAccountRepository accountRepository, 
            IUserRepository userRepository,
            ICurrencyConverter converter)
        {
            this.unitOfWork = unitOfWork;
            this.userValidator = userValidator;
            this.bankAccountValidator = bankAccountValidator;
            this.historyRepository = historyRepository;
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.converter = converter;
        }

        public async Task CloseBankAccountByIdAsync(string id)
        {
            var account = await accountRepository.GetBankAccountByIdAsync(id);
            bankAccountValidator.ValidateAndThrow(account, options => options.IncludeRuleSets("Close"));

            account.IsClosed = true;
            account.ClosingDate = DateTime.Now;

            if (!await accountRepository.UpdateBankAccountAsync(account))
            {
                throw new ValidationException("Банковский аккаунт с переданным идентификатором не существует");
            }
            unitOfWork.SaveChanges();
        }

        public async Task<decimal> GetTransferCommissionAsync(decimal? amount, string fromAccountId, string toAccountId)
        {
            var validAmount = ValidateAmountAndThrow(amount);

            var source = await accountRepository.GetBankAccountByIdAsync(fromAccountId);
            bankAccountValidator.ValidateAndThrow(source);

            var destination = await accountRepository.GetBankAccountByIdAsync(toAccountId);
            bankAccountValidator.ValidateAndThrow(destination);

            if (source.UserId != destination.UserId)
            {
                var commission = validAmount / 100 * commissionPercentage;
                return Math.Round(commission, 2);
            }

            return 0;
        }

        public async Task<string> CreateBankAccountAsync(string userId, string currencyCode)
        {
            userValidator.ValidateAndThrow(await userRepository.GetUserByIdAsync(userId));

            if (!Enum.IsDefined(typeof(ValidCurrencies), currencyCode))
            {
                throw new ValidationException($"{currencyCode} - Недопустимый валютный код");
            }

            var accuntId = await accountRepository.CreateBankAccountAsync(userId, currencyCode);
            unitOfWork.SaveChanges();
            return accuntId;
        }

        public async Task UpdateFundsTransferAsync(decimal? amount, string fromAccountId, string toAccountId)
        {
            var validAmount = ValidateAmountAndThrow(amount);
            decimal commission = 0;

            var source = await accountRepository.GetBankAccountByIdAsync(fromAccountId);
            var destination = await accountRepository.GetBankAccountByIdAsync(toAccountId);

            if (source.UserId != destination.UserId)
            {
                commission = Math.Round(validAmount / 100 * commissionPercentage, 2);
            }

            await WithdrawFundsFromSourceAccountAsync(validAmount, source);

            await TransferFundsToDestinationAccountAsync(validAmount, commission, source, destination);

            await historyRepository.CreateMoneyTransfersHistoryAsync(new MoneyTransferHistory
            {
                Amount = validAmount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
            });
            unitOfWork.SaveChanges();
        }

        private async Task WithdrawFundsFromSourceAccountAsync(decimal amount, BankAccount source)
        {
            if (source.Amount - amount >= 0)
            {
                source.Amount -= amount;
                if (!await accountRepository.UpdateBankAccountAsync(source))
                {
                    throw new ValidationException("Не удалось совершить перевод");
                }
                return;
            }

            throw new ValidationException(
                $"Недостаточно средств для осуществления перевода, баланс: {source.Amount} {source.CurrencyCode}"
            );
        }
        
        private async Task TransferFundsToDestinationAccountAsync(
            decimal initialAmount, 
            decimal initialCommission, 
            BankAccount source,
            BankAccount destination)
        {
            var amountWithoutCommission = initialAmount - initialCommission;
            var transferAmount = GetMoneyInNewCurrency(amountWithoutCommission, source.CurrencyCode, destination.CurrencyCode);

            destination.Amount += transferAmount;
            if (! await accountRepository.UpdateBankAccountAsync(destination))
            {
                throw new ValidationException("Не удалось совершить перевод");
            }
        }

        private decimal GetMoneyInNewCurrency(
            decimal amount, 
            string fromCurrency, 
            string toCurrency)
        {
            if (fromCurrency != toCurrency)
            {
                return converter.Convert(amount, fromCurrency, toCurrency);
            }

            return amount;
        }

        private decimal ValidateAmountAndThrow(decimal? amount)
        {
            if (amount == null)
            {
                throw new ValidationException("Передана пустая сумма");
            }

            return (decimal)amount;
        }
    }
}
