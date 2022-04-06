using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domains.BankAccounts.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> GetBankAccountByIdAsync(string id, CancellationToken cancellation);
        Task<bool> UpdateBankAccountAsync(BankAccount bankAccount, CancellationToken cancellation);
        Task<bool> CheckIsBankAccountByUserIdExistAsync(string userId, CancellationToken cancellation);
        Task<string> CreateBankAccountAsync(string userId, string currencyCode, CancellationToken cancellation);
    }
}
