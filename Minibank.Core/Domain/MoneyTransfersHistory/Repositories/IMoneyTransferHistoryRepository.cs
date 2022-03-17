using System;

namespace Minibank.Core.Domains.MoneyTransfersHistory.Repositories
{
    public interface IMoneyTransferHistoryRepository
    {
        void PostMoneyTransfersHistory(MoneyTransferHistory history);
    }
}
