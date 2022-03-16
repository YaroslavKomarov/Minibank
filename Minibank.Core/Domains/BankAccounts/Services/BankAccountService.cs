using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.MoneyTransfersHistory;
using Minibank.Core.Domains.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Services;
using System;
using System.Collections.Generic;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    internal class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository accountRepository;

        private readonly IMoneyTransferHistoryRepository historyRepository;

        private readonly IUserRepository userRepository;

        private readonly ICurrencyConverter converter;

        private static readonly int commissionPercentage = 2;

        private static readonly HashSet<string> validCurrencies = new HashSet<string>
        {
            "RUB",
            "USD",
            "EUR"
        };

        public BankAccountService(
            IMoneyTransferHistoryRepository historyRepository,
            IBankAccountRepository accountRepository, 
            IUserRepository userRepository,
            ICurrencyConverter converter)
        {
            this.historyRepository = historyRepository;
            this.accountRepository = accountRepository;
            this.userRepository = userRepository;
            this.converter = converter;
        }

        public void CloseBankAccountById(string id)
        {
            var account = accountRepository.GetBankAccountById(id);

            if (account == null) 
            {
                throw new ValidationException("Аккаунт с переданным идентефикатором не существует");
            }

            if (account.Amount != 0) 
            {
                throw new ValidationException("Невозможно удалить аккаунт с ненулевым счетом");
            }

            account.IsClosed = true;
        }

        public decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId)
        {
            var validAmount = ValidateAmount(amount);

            var source = accountRepository.GetBankAccountById(fromAccountId);
            var destination = accountRepository.GetBankAccountById(toAccountId);

            ValidateAccount(source);
            ValidateAccount(destination);

            if (source.UserId != destination.UserId)
            {
                var commission = validAmount / 100 * commissionPercentage;
                return Math.Round(commission, 2);
            }

            return 0;
        }

        public void CreateBankAccount(string userId, string currencyCode)
        {
            var user = userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new ValidationException("Пользователь с переданным идентефикатором не существует");
            }

            if (!validCurrencies.Contains(currencyCode))
            {
                throw new ValidationException($"Недопустимый валютный код: {currencyCode}");
            }

            accountRepository.CreateBankAccount(userId, currencyCode);
        }

        public void UpdateFundsTransfer(decimal? amount, string fromAccountId, string toAccountId)
        {
            var validAmount = ValidateAmount(amount);

            var commission = GetTransferCommission(validAmount, fromAccountId, toAccountId);

            var source = accountRepository.GetBankAccountById(fromAccountId);
            var destination = accountRepository.GetBankAccountById(toAccountId);

            WithdrawFundsFromSourceAccount(validAmount, source);

            TransferFundsToDestinationAccount(validAmount, commission, source, destination);

            historyRepository.PostMoneyTransfersHistory(new MoneyTransferHistory
            {
                Amount = validAmount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
            });
        }

        private void WithdrawFundsFromSourceAccount(decimal amount, BankAccount sourceAccount)
        {
            if (sourceAccount.Amount - amount >= 0)
            {
                sourceAccount.Amount -= amount;
            }

            throw new ValidationException(
                $"Недостаточно средств для осуществления перевода, баланс: {sourceAccount.Amount} {sourceAccount.CurrencyCode}"
            );
        }
        
        private void TransferFundsToDestinationAccount(
            decimal initialAmount, 
            decimal initialCommission, 
            BankAccount source,
            BankAccount destination)
        {
            var amountWithoutCommission = initialAmount - initialCommission;
            var transferAmount = GetMoneyInNewCurrency(amountWithoutCommission, source.CurrencyCode, destination.CurrencyCode);

            destination.Amount += transferAmount;
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

        private void ValidateAccount(BankAccount sourceAccount)
        {
            if (sourceAccount == null)
            {
                throw new ValidationException("Аккаунт с переданным идентефикатором не существует");
            }
        }

        private decimal ValidateAmount(decimal? amount)
        {
            if (amount == null)
            {
                throw new ValidationException("Передана пустая сумма");
            }
            return (decimal)amount;
        }
    }
}
