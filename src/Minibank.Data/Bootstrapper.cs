using Minibank.Core.Domain.MoneyTransfersHistory.Repositories;
using Minibank.Data.MoneyTransfersHistory.Repositories;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Core.Domain.Currency.Services;
using Microsoft.Extensions.Configuration;
using Minibank.Data.Users.Repositories;
using Minibank.Data.Currency.Services;
using Microsoft.EntityFrameworkCore;
using Minibank.Core;
using System;

namespace Minibank.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

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
