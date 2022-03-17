using System;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        void CreateBankAccount(string userId, string currencyCode);
        void CloseBankAccountById(string id);
        decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId);
        void UpdateFundsTransfer(decimal? amount, string fromAccountId, string toAccountId);
    }
}
