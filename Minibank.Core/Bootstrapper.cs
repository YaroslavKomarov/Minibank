using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.BankAccounts.Services;
using Minibank.Core.Domains.Users.Services;
using Minibank.Core.Services;

namespace Minibank.Core
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            return services;
        }
    }
}
