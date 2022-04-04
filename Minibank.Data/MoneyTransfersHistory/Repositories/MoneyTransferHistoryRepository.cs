using Minibank.Core.Domains.MoneyTransfersHistory;
using Minibank.Core.Domains.MoneyTransfersHistory.Repositories;
using System;
using System.Threading.Tasks;

namespace Minibank.Data.MoneyTransfersHistory.Repositories
{
    public class MoneyTransferHistoryRepository : IMoneyTransferHistoryRepository
    {
        private readonly MinibankContext context;

        public MoneyTransferHistoryRepository(MinibankContext context)
        {
            this.context = context;
        }

        public async Task CreateMoneyTransfersHistoryAsync(MoneyTransferHistory history)
        {
            var moneyTransferHistoryModel = new MoneyTransferHistoryDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = history.Amount,
                CurrencyCode = history.CurrencyCode,
                FromAccountId = history.FromAccountId,
                ToAccountId = history.ToAccountId
            };

            await context.MoneyTransferHistories.AddAsync(moneyTransferHistoryModel);
        }
    }
}
