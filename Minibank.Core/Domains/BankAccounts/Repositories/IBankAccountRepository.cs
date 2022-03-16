using System;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccount GetBankAccountById(string id);
        BankAccount GetBankAccountByUserId(string userId);
        void PostBankAccount(string userId, string currencyCode);
        bool DeleteBankAccountById(string id);
    }
}
