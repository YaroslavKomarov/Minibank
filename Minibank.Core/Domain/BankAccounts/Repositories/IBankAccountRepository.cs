using System;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        BankAccount GetBankAccountById(string id);
        bool UpdateBankAccount(BankAccount bankAccount);
        bool ExistBankAccountByUserId(string userId);
        string CreateBankAccount(string userId, string currencyCode);
    }
}
