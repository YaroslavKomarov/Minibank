using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using Minibank.Core.Domain.BankAccounts;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MinibankContext context;

        public BankAccountRepository(MinibankContext context)
        {
            this.context = context;
        }

        public async Task<BankAccount> GetBankAccountByIdAsync(
            string id, CancellationToken cancellationToken)
        {
            var accountModel = await context.BancAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (accountModel == null)
            {
                return null;
            }

            return new BankAccount
            {
                Id = accountModel.Id,
                UserId = accountModel.UserId,
                Amount = accountModel.Amount,
                CurrencyCode = accountModel.CurrencyCode,
                OpeningDate = accountModel.OpeningDate,
                ClosingDate = accountModel.ClosingDate,
                IsClosed = accountModel.IsClosed
            };
        }

        public async Task<bool> CheckDoesNotBankAccountExistByUserIdAsync(
            string userId, CancellationToken cancellationToken)
        {
            return !await context.BancAccounts
                .AsNoTracking()
                .AnyAsync(it => it.UserId == userId, cancellationToken);
        }

        public async Task<string> CreateBankAccountAsync(
            string userId, string currencyCode, CancellationToken cancellationToken)
        {
            var entry = await context.BancAccounts.AddAsync(new BankAccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CurrencyCode = currencyCode.ToUpperInvariant(),
                OpeningDate = DateTime.Now,
                Amount = 1000
            }, cancellationToken);

            return entry.Entity.Id;
        }

        public async Task<bool> UpdateBankAccountAsync(
            BankAccount bankAccount, CancellationToken cancellationToken)
        {
            var oldAccountModel = await context.BancAccounts
                .FirstOrDefaultAsync(it => it.Id == bankAccount.Id, cancellationToken);

            if (oldAccountModel != null)
            {
                context.Entry(oldAccountModel).CurrentValues.SetValues(bankAccount);

                return true;
            } 

            return false;
        }
    }
}
