using System;

namespace Minibank.Core.Domains.MoneyTransfersHistory.Repositories
{
    public interface IMoneyTransferHistoryRepository
    {
        void CreateMoneyTransfersHistory(MoneyTransferHistory history);
    }
}
