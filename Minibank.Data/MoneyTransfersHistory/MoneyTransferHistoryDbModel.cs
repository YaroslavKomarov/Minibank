using System;

namespace Minibank.Data.MoneyTransfersHistory
{
    public class MoneyTransferHistoryDbModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
