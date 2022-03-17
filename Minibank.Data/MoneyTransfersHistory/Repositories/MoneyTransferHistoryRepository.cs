using Minibank.Core.Domains.MoneyTransfersHistory;
using Minibank.Core.Domains.MoneyTransfersHistory.Repositories;
using System;
using System.Collections.Generic;

namespace Minibank.Data.MoneyTransfersHistory.Repositories
{
    public class MoneyTransferHistoryRepository : IMoneyTransferHistoryRepository
    {
        private static List<MoneyTransferHistoryDbModel> moneyTransferHistoryStorage = new List<MoneyTransferHistoryDbModel>();

        public void CreateMoneyTransfersHistory(MoneyTransferHistory history)
        {
            var moneyTransferHistoryModel = new MoneyTransferHistoryDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = history.Amount,
                CurrencyCode = history.CurrencyCode,
                FromAccountId = history.FromAccountId,
                ToAccountId = history.ToAccountId
            };

            moneyTransferHistoryStorage.Add(moneyTransferHistoryModel);
        }
    }
}
