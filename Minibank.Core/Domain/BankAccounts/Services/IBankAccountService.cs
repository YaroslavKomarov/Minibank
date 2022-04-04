using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<string> CreateBankAccountAsync(string userId, string currencyCode);
        Task CloseBankAccountByIdAsync(string id);
        Task<decimal> GetTransferCommissionAsync(decimal? amount, string fromAccountId, string toAccountId);
        Task UpdateFundsTransferAsync(decimal? amount, string fromAccountId, string toAccountId);
    }
}
