namespace Minibank.Web.Controllers.BankAccounts
{
    public class TransferDto
    {
        public decimal? amount { get; set; }
        public string fromAccountId { get; set; }
        public string toAccountId { get; set; }
    }
}
