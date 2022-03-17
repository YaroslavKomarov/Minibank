using System;

namespace Minibank.Core.Services
{
    public class ValidationException : Exception
    {
        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
    }
}
