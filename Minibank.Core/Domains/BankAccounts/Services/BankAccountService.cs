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

        public void DeleteBankAccountById(string id)
        {
            var account = accountRepository.GetBankAccountById(id);

            if (account == null || account.Amount != 0)
            {
                throw new ValidationException("Невозможно удалить аккаунт с ненулевым счетом");
            }

            accountRepository.DeleteBankAccountById(id);
        }

        public decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId)
        {
            if (amount == null)
            {
                throw new ValidationException("Передана пустая сумма");
            }
            var validAmount = (decimal)amount;

            var sourceAccount = accountRepository.GetBankAccountById(fromAccountId);
            var destinationAccount = accountRepository.GetBankAccountById(toAccountId);

            ValidateTransferAccounts(sourceAccount, destinationAccount);

            if (sourceAccount.UserId != destinationAccount.UserId)
            {
                var commission = validAmount / 100 * commissionPercentage;
                return Math.Round(commission, 2);
            }

            return 0;
        }

        public void PostBankAccount(string userId, string currencyCode)
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

            accountRepository.PostBankAccount(userId, currencyCode);
        }

        public void PutFundsTransfer(decimal? amount, string fromAccountId, string toAccountId)
        {
            if (amount == null)
            {
                throw new ValidationException("Передана пустая сумма");
            }
            var validAmount = (decimal)amount;

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
            var amount = GetMoneyInNewCurrency(initialAmount, source.CurrencyCode, destination.CurrencyCode);
            var commission = GetMoneyInNewCurrency(initialCommission, source.CurrencyCode, destination.CurrencyCode);

            destination.Amount += amount - commission;
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

        private void ValidateTransferAccounts(BankAccount sourceAccount, BankAccount destinationAccount)
        {
            if (sourceAccount == null)
            {
                throw new ValidationException("Аккаунт источника с переданным идентефикатором не существует");
            }

            if (destinationAccount == null)
            {
                throw new ValidationException("Аккаунт назначения с переданным идентефикатором не существует");
            }
        }
    }
}
