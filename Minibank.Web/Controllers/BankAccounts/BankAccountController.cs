using Minibank.Core.Domains.BankAccounts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task CloseBankAccountByIdAsync([FromBody] CloseBankAccountDto model)
        {
            await accountService.CloseBankAccountByIdAsync(model.Id);
        }

        [HttpGet]
        public async Task<decimal> GetTransferCommissionAsync(decimal? amount, string fromAccountId, string toAccountId)
        {
            return await accountService.GetTransferCommissionAsync(amount, fromAccountId, toAccountId);
        }

        [HttpPost]
        public async Task<string> CreateBankAccountASync([FromBody] CreateBankAccountDto model)
        {
            return await accountService.CreateBankAccountAsync(model.UserId, model.CurrencyCode);
        }

        [HttpPut]
        public async Task UpdateTransferFundsASync([FromBody] TransferDto model)
        {
            await accountService.UpdateFundsTransferAsync(
                model.Amount,
                model.FromAccountId,
                model.ToAccountId);
        }
    }
}
