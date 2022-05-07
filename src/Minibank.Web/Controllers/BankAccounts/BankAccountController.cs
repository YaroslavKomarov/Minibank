using Minibank.Core.Domains.BankAccounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;

namespace Minibank.Web.Controllers.BankAccounts
{
    [ApiController]
    [Authorize]
    [Route("api/v1/minibank/[controller]/[action]")]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService accountService;

        public BankAccountController(IBankAccountService service)
        {
            accountService = service;
        }

        [HttpPost]
        public async Task CloseBankAccountByIdAsync([FromBody] CloseBankAccountDto model, CancellationToken cancellationToken)
        {
            await accountService.CloseBankAccountByIdAsync(model.Id, cancellationToken);
        }

        [HttpGet]
        public async Task<string> GetTransferCommissionAsync(decimal? amount, string fromAccountId, string toAccountId, CancellationToken cancellationToken)
        {
            return await accountService.GetTransferCommissionAsync(
                amount,
                fromAccountId,
                toAccountId,
                cancellationToken);
        }

        [HttpPost]
        public async Task<string> CreateBankAccountAsync([FromBody] CreateBankAccountDto model, CancellationToken cancellationToken)
        {
            return await accountService.CreateBankAccountAsync(
                model.UserId,
                model.CurrencyCode,
                cancellationToken);
        }

        [HttpPut]
        public async Task UpdateTransferFundsASync([FromBody] TransferDto model, CancellationToken cancellationToken)
        {
            await accountService.UpdateFundsTransferAsync(
                model.Amount,
                model.FromAccountId,
                model.ToAccountId,
                cancellationToken);
        }
    }
}
