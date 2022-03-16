using System;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        void PostBankAccount(string userId, string currencyCode);
        void DeleteBankAccountById(string id);
        decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId);
        void PutFundsTransfer(decimal? amount, string fromAccountId, string toAccountId);
    }
}
