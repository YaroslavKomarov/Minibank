using FluentValidation.Internal;
using FluentValidation;
using System.Linq;
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
                var errMessage = result.Errors.FirstOrDefault().ErrorMessage;

                throw new FluentValidation.ValidationException(errMessage);
            }
        }
    }
}
