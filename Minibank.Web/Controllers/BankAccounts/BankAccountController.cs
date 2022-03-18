using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts.Services;

namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService accountService;

        public BankAccountController(IBankAccountService service)
        {
            accountService = service;
        }

        [HttpDelete]
        public void CloseBankAccountById(string id)
        {
            accountService.CloseBankAccountById(id);
        }

        [HttpGet]
        public decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId)
        {
            return accountService.GetTransferCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost]
        public void CreateBankAccount([FromBody] CreateBankAccountDto model)
        {
            accountService.CreateBankAccount(model.UserId, model.CurrencyCode);
        }

        [HttpPut]
        public void UpdateTransferFunds([FromBody] TransferDto model)
        {
            accountService.UpdateFundsTransfer(
                model.Amount,
                model.FromAccountId,
                model.ToAccountId);
        }
    }
}
