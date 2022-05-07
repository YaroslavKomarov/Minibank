using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Core.Domain.MoneyTransfersHistory.Repositories
{
    public interface IMoneyTransferHistoryRepository
    {
        Task CreateMoneyTransfersHistoryAsync(
            MoneyTransferHistory history, 
            CancellationToken cancellationToken);
    }
}
