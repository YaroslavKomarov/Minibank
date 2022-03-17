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
        public decimal GetTransferCommission(TransferDto model)
        {
            return accountService.GetTransferCommission(
                model.amount, 
                model.fromAccountId, 
                model.toAccountId);
        }

        [HttpPost]
        public void CreateBankAccount(CreateBankAccountDto model)
        {
            accountService.CreateBankAccount(model.UserId, model.CurrencyCode);
        }

        [HttpPut]
        public void UpdateTransferFunds(TransferDto model)
        {
            accountService.UpdateFundsTransfer(
                model.amount,
                model.fromAccountId,
                model.toAccountId);
        }
    }
}
