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

        public bool DeleteBankAccountById(string id)
        {
            var accountModel = bankAccountStorage.FirstOrDefault(it => it.Id == id);

            if (accountModel != null)
            {
                bankAccountStorage.Remove(accountModel);
                return true;
            }

            return false;
        }

        public BankAccount GetBankAccountByUserId(string userId)
        {
            var accountModel = bankAccountStorage.FirstOrDefault(it => it.UserId == userId);

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

        public void PostBankAccount(string userId, string currencyCode)
        {
            var accountModel = new BankAccountDbModel
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CurrencyCode = currencyCode,
                Amount = 1000
            };

            bankAccountStorage.Add(accountModel);
        }
    }
}
