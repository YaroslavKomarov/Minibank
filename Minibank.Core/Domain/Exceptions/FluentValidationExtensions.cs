using FluentValidation.Internal;
using FluentValidation;
using System;

namespace Minibank.Core.Domain.Exceptions
{
    public static class FluentValidationExtensions
    {
        public static void ValidateAndThrow<T>(this IValidator<T> validator, T instance, Action<ValidationStrategy<T>> options)
        {
            var result = validator.Validate(instance, options);

            if (!result.IsValid)
            {
                throw new FluentValidation.ValidationException(result.Errors);
            }
        }
    }
}
