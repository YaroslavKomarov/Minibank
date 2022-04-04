using System.Threading.Tasks;

namespace Minibank.Core.Domains.MoneyTransfersHistory.Repositories
{
    public interface IMoneyTransferHistoryRepository
    {
        Task CreateMoneyTransfersHistoryAsync(MoneyTransferHistory history);
    }
}
