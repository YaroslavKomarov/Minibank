using System;

namespace Minibank.Core.Services
{
    public class InvalidCurrencyArgumentsException : Exception
    {
        public InvalidCurrencyArgumentsException(string message) : base(message) { }
    }
}
