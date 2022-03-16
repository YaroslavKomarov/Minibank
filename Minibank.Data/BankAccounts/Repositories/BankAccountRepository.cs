using Minibank.Core.Domains.BankAccounts.Repositories;
using System.Collections.Generic;
using System;
using System.Linq;
using Minibank.Core.Domains.BankAccounts;

namespace Minibank.Data.BankAccounts.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private static List<BankAccountDbModel> bankAccountStorage = new List<BankAccountDbModel>();

        public BankAccount GetBankAccountById(string id)
        {
            var accountModel = bankAccountStorage.FirstOrDefault(it => it.Id == id);

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

        public bool ExistBankAccountByUserId(string userId)
        {
            var accountModel = bankAccountStorage.FirstOrDefault(it => it.UserId == userId);

            return accountModel == null ? false : true;
        }

        public void CreateBankAccount(string userId, string currencyCode)
        {
            var accountModel = new BankAccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CurrencyCode = currencyCode,
                OpeningDate = DateTime.Now,
                Amount = 1000
            };

            bankAccountStorage.Add(accountModel);
        }

        public bool UpdateBankAccount(BankAccount bankAccount)
        {
            var oldAccountModel = bankAccountStorage.FirstOrDefault(it => it.Id == bankAccount.Id);

            if (oldAccountModel != null)
            {
                bankAccountStorage[bankAccountStorage.IndexOf(oldAccountModel)] = new BankAccountDbModel
                {
                    Id = bankAccount.Id,
                    UserId = bankAccount.UserId,
                    Amount = bankAccount.Amount,
                    CurrencyCode = bankAccount.CurrencyCode,
                    OpeningDate = bankAccount.OpeningDate,
                    ClosingDate = bankAccount.ClosingDate,
                    IsClosed = bankAccount.IsClosed
                };
                return true;
            }

            return false;
        }
    }
}
