using System.Threading.Tasks;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountByIdAsync(string id);
        Task<bool> UpdateBankAccountAsync(BankAccount bankAccount);
        Task<bool> CheckIsBankAccountByUserIdExistAsync(string userId);
        Task<string> CreateBankAccountAsync(string userId, string currencyCode);
    }
}
