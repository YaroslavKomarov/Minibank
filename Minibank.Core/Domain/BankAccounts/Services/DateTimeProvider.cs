using System;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
