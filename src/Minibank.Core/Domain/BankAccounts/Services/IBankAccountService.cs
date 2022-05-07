using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domains.BankAccounts.Services
{
    public interface IBankAccountService
    {
        Task<string> CreateBankAccountAsync(
            string userId,
            string currencyCode,
            CancellationToken cancellationToken);
        Task CloseBankAccountByIdAsync(
            string id,
            CancellationToken cancellationToken);
        Task<string> GetTransferCommissionAsync(
            decimal? amount,
            string fromAccountId, 
            string toAccountId,
            CancellationToken cancellationToken);
        Task UpdateFundsTransferAsync(
            decimal? amount,
            string fromAccountId, 
            string toAccountId,
            CancellationToken cancellationToken);
    }
}
