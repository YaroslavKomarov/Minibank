using System;

namespace Minibank.Web.Controllers.BankAccounts.DTO
{
    public class BankAccountDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool IsClosed { get; set; }
    }
}
