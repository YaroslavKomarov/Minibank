using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.BankAccounts.Services;
using System;

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
        public void DeleteBankAccountById(string id)
        {
            accountService.DeleteBankAccountById(id);
        }

        [HttpGet]
        public decimal GetTransferCommission(decimal amount, string fromAccountId, string toAccountId)
        {
            return accountService.GetTransferCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost]
        public void PostBankAccount(string userId, string currencyCode)
        {
            accountService.PostBankAccount(userId, currencyCode);
        }

        [HttpPut]
        public void PutTransferFunds(decimal amount, string fromAccountId, string toAccountId)
        {
            accountService.PutFundsTransfer(amount, fromAccountId, toAccountId);
        }
    }
}
