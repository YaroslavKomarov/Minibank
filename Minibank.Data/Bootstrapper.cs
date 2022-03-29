using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Core.Services;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Data.Currency.Services;
using Minibank.Data.MoneyTransfersHistory.Repositories;
using Minibank.Data.Users.Repositories;
using System;

namespace Minibank.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICurrencyRate, CurrencyRate>(options =>
            {
                options.BaseAddress = new Uri(configuration["CbrCurrenciesUri"]);
            });
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IMoneyTransferHistoryRepository, MoneyTransferHistoryRepository>();

            return services;
        }
    }
}
