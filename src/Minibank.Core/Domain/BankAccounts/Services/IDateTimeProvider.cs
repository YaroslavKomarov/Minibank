using System;

namespace Minibank.Core.Domain.BankAccounts.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
