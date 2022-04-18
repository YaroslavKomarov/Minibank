using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;
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
            services.AddHttpClient<ICurrencyRateService, CurrencyRateService>(options =>
            {
                options.BaseAddress = new Uri(configuration["CbrCurrenciesUri"]);
            });
            services.AddScoped<IMoneyTransferHistoryRepository, MoneyTransferHistoryRepository>();

            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddDbContext<MinibankContext>(options => options.UseNpgsql(configuration["DbConnectionString"]));

            return services;
        }
    }
}
