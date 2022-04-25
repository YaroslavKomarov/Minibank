using ValidationException = Minibank.Core.Domain.Exceptions.ValidationException;
using Minibank.Core.Domain.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domain.BankAccounts.Services;
using Minibank.Core.Domain.MoneyTransfersHistory;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domain.Exceptions;
using Minibank.Core.Domain.Currency;
using Minibank.Core.Domains.Users;
using System.Threading.Tasks;
using FluentValidation;
using System.Threading;
using System;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IValidator<User> userValidator;

        private readonly IValidator<BankAccount> bankAccountValidator;

        private readonly IDateTimeProvider dateTimeProvider;

        private readonly ICurrencyConverterService converter;

        private readonly IUserRepository userRepository;

        private readonly IBankAccountRepository accountRepository;

        private readonly IMoneyTransferHistoryRepository historyRepository;

        private const int commissionPercentage = 2;

        public BankAccountService(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IValidator<User> userValidator,
            IValidator<BankAccount> bankAccountValidator,
            IMoneyTransferHistoryRepository historyRepository,
            IBankAccountRepository accountRepository,
            IUserRepository userRepository,
            ICurrencyConverterService converter)
        {
            this.unitOfWork = unitOfWork;
            this.userValidator = userValidator;
            this.bankAccountValidator = bankAccountValidator;
            this.historyRepository = historyRepository;
            this.accountRepository = accountRepository;
            this.dateTimeProvider = dateTimeProvider;
            this.userRepository = userRepository;
            this.converter = converter;
        }

        public async Task CloseBankAccountByIdAsync(string id, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetBankAccountByIdAsync(id, cancellationToken);
            bankAccountValidator.ValidateAndThrow(account, options => options.IncludeRuleSets("Close"));

            account.IsClosed = true;
            account.ClosingDate = dateTimeProvider.Now;
            
            if (!await accountRepository.UpdateBankAccountAsync(account, cancellationToken))
            {
                throw new ValidationException("Не удалось закрыть банковский аккаунт");
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetTransferCommissionAsync(
            decimal? amount,
            string fromAccountId,
            string toAccountId,
            CancellationToken cancellationToken)
        {
            var source = await accountRepository.GetBankAccountByIdAsync(fromAccountId, cancellationToken);
            var destination = await accountRepository.GetBankAccountByIdAsync(toAccountId, cancellationToken);
            var validAmount = ValidateTransferDataAndThrow(source, destination, amount);

            var commission = CalculateTransferCommission(source, destination, validAmount);

            return $"{commission} {source.CurrencyCode}";
        }

        public async Task<string> CreateBankAccountAsync(
            string userId,
            string currencyCode, 
            CancellationToken cancellationToken)
        {
            userValidator.ValidateAndThrow(await userRepository.GetUserByIdAsync(userId, cancellationToken));

            if (!Enum.IsDefined(typeof(ValidCurrencies), currencyCode.ToUpperInvariant()))
            {
                throw new ValidationException($"{currencyCode} - Недопустимый валютный код");
            }

            var accountId = await accountRepository.CreateBankAccountAsync(
                userId, currencyCode, cancellationToken);

            await unitOfWork.SaveChangesAsync();
            return accountId;
        }

        public async Task UpdateFundsTransferAsync(
            decimal? amount, 
            string fromAccountId, 
            string toAccountId,
            CancellationToken cancellationToken)
        {
            var source = await accountRepository.GetBankAccountByIdAsync(fromAccountId, cancellationToken);
            var destination = await accountRepository.GetBankAccountByIdAsync(toAccountId, cancellationToken);

            var validAmount = ValidateTransferDataAndThrow(source, destination, amount);
            var commission = CalculateTransferCommission(source, destination, validAmount);

            await WithdrawFundsFromSourceAccountAsync(validAmount, source, cancellationToken);

            await TransferFundsToDestinationAccountAsync(
                validAmount, commission, source, destination, cancellationToken);

            await historyRepository.CreateMoneyTransfersHistoryAsync(new MoneyTransferHistory
            {
                Amount = validAmount,
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                CurrencyCode = source.CurrencyCode
            }, cancellationToken);

            await unitOfWork.SaveChangesAsync();
        }

        private decimal ValidateTransferDataAndThrow(
            BankAccount source, BankAccount destination, decimal? amount)
        {
            bankAccountValidator.ValidateAndThrow(source);
            bankAccountValidator.ValidateAndThrow(destination);

            if (!amount.HasValue || amount.Value < 0)
            {
                throw new ValidationException("Невалидное значение суммы");
            }
            return amount.Value;
        }

        private decimal CalculateTransferCommission(
            BankAccount source, BankAccount destination, decimal amount)
        {
            if (source.UserId != destination.UserId)
            {
                var commission = amount / 100 * commissionPercentage;
                return Math.Round(commission, 2);
            }
            return 0;
        }

        private async Task WithdrawFundsFromSourceAccountAsync(
            decimal amount,
            BankAccount source,
            CancellationToken cancellationToken)
        {
            if (source.Amount - amount >= 0)
            {
                source.Amount -= amount;
                if (!await accountRepository.UpdateBankAccountAsync(source, cancellationToken))
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
            BankAccount destination,
            CancellationToken cancellationToken)
        {
            var amountWithoutCommission = initialAmount - initialCommission;
            var transferAmount = await GetMoneyInNewCurrency(
                amountWithoutCommission, 
                source.CurrencyCode, 
                destination.CurrencyCode,
                cancellationToken);

            destination.Amount += transferAmount;
            if (! await accountRepository.UpdateBankAccountAsync(destination, cancellationToken))
            {
                throw new ValidationException("Не удалось совершить перевод");
            }
        }

        private async Task<decimal> GetMoneyInNewCurrency(
            decimal amount, 
            string fromCurrency, 
            string toCurrency,
            CancellationToken cancellationToken)
        {
            if (fromCurrency != toCurrency)
            {
                return await converter.Convert(
                    amount, 
                    fromCurrency, 
                    toCurrency, 
                    cancellationToken);
            }
            return amount;
        }
    }
}
