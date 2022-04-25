using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domain.BankAccounts.Services;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domain.Currency.Services;
using Minibank.Core.Domains.Users.Services;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace Minibank.Core
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();

            services.AddScoped<IBankAccountService, BankAccountService>();

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUserService, UserService>();

            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(Bootstrapper).Assembly);

            return services;
        }
    }
}
