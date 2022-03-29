namespace Minibank.Web.Controllers.BankAccounts
{
    public class TransferDto
    {
        public decimal? Amount { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
