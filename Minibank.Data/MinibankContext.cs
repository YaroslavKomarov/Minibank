using Microsoft.EntityFrameworkCore;
using Minibank.Data.BankAccounts;
using Minibank.Data.Users;
using System;
using Minibank.Data.MoneyTransfersHistory;
using Microsoft.EntityFrameworkCore.Design;

namespace Minibank.Data
{
    public class MinibankContext : DbContext
    {
        public DbSet<UserDbModel> Users { get; set; }

        public DbSet<BankAccountDbModel> BancAccounts { get; set; }

        public DbSet<MoneyTransferHistoryDbModel> MoneyTransferHistories { get; set; }

        public MinibankContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MinibankContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder); 
        }
    }

    public class Factory : IDesignTimeDbContextFactory<MinibankContext>
    {
        public MinibankContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder()
                .UseNpgsql("ConnectionString")
                .Options;

            return new MinibankContext(options);
        }
    }
}
