﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public void CloseBankAccountById([FromBody] CloseBankAccountDto model)
        {
            accountService.CloseBankAccountById(model.Id);
        }

        [HttpGet]
        public decimal GetTransferCommission(decimal? amount, string fromAccountId, string toAccountId)
        {
            return accountService.GetTransferCommission(amount, fromAccountId, toAccountId);
        }

        [HttpPost]
        public string CreateBankAccount([FromBody] CreateBankAccountDto model)
        {
            return accountService.CreateBankAccount(model.UserId, model.CurrencyCode);
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