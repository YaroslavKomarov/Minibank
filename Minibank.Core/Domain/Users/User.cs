using Minibank.Core.Domains.BankAccounts;
using System.Collections.Generic;

namespace Minibank.Core.Domains.Users
{
    public class User
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
