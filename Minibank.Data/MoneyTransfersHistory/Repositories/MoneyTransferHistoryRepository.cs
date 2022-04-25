using Minibank.Core.Domain.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domain.MoneyTransfersHistory;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Minibank.Data.MoneyTransfersHistory.Repositories
{
    public class MoneyTransferHistoryRepository : IMoneyTransferHistoryRepository
    {
        private readonly MinibankContext context;

        public MoneyTransferHistoryRepository(MinibankContext context)
        {
            this.context = context;
        }

        public async Task CreateMoneyTransfersHistoryAsync(
            MoneyTransferHistory history,
            CancellationToken cancellationToken)
        {
            var moneyTransferHistoryModel = new MoneyTransferHistoryDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Amount = history.Amount,
                CurrencyCode = history.CurrencyCode,
                FromAccountId = history.FromAccountId,
                ToAccountId = history.ToAccountId
            };

            await context.MoneyTransferHistories
                .AddAsync(moneyTransferHistoryModel, cancellationToken);
        }
    }
}
