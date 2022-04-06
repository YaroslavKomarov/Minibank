using Minibank.Core.Domains.BankAccounts.Services;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.Users.Services;
using FluentValidation.AspNetCore;
using Minibank.Core.Services;
using FluentValidation;

namespace Minibank.Core
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();

            services.AddScoped<IBankAccountService, BankAccountService>();

            services.AddScoped<IUserService, UserService>();

            services.AddFluentValidation().AddValidatorsFromAssembly(typeof(UserService).Assembly);

            return services;
        }
    }
}
