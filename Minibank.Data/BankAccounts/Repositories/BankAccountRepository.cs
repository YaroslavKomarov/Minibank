using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.BankAccounts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly MinibankContext context;

        public BankAccountRepository(MinibankContext context)
        {
            this.context = context;
        }

        public async Task<BankAccount> GetBankAccountByIdAsync(string id)
        {
            var accountModel = await context.BancAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == id);

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

        public async Task<bool> CheckIsBankAccountByUserIdExistAsync(string userId)
        {
            var accountModel = await context.BancAccounts
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.UserId == userId);

            return accountModel == null ? false : true;
        }

        public async Task<string> CreateBankAccountAsync(string userId, string currencyCode)
        {
            var id = Guid.NewGuid().ToString();

            await context.BancAccounts.AddAsync(new BankAccountDbModel
            {
                Id = id,
                UserId = userId,
                CurrencyCode = currencyCode,
                OpeningDate = DateTime.Now,
                Amount = 1000
            });

            return id;
        }

        public async Task<bool> UpdateBankAccountAsync(BankAccount bankAccount)
        {
            var oldAccountModel = await context.BancAccounts
                .FirstOrDefaultAsync(it => it.Id == bankAccount.Id);

            if (oldAccountModel != null)
            {
                oldAccountModel.UserId = bankAccount.UserId;
                oldAccountModel.Amount = bankAccount.Amount;
                oldAccountModel.CurrencyCode = bankAccount.CurrencyCode;
                oldAccountModel.OpeningDate = bankAccount.OpeningDate;
                oldAccountModel.ClosingDate = bankAccount.ClosingDate;
                oldAccountModel.IsClosed = bankAccount.IsClosed;

                return true;
            }

            return false;
        }
    }
}
