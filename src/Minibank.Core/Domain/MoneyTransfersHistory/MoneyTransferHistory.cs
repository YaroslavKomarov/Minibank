using System;

namespace Minibank.Core.Domain.MoneyTransfersHistory
{
    public class MoneyTransferHistory
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode{ get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
