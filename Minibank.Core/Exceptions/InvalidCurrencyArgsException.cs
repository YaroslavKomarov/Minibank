using System;

namespace Minibank.Core.Services
{
    public class InvalidCurrencyArgsException : Exception
    {
        public InvalidCurrencyArgsException(string message) : base(message) { }
    }
}
