using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Minibank.Data.BankAccounts
{
    public class BankAccountDbModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public bool IsClosed { get; set; }

        internal class Map : IEntityTypeConfiguration<BankAccountDbModel>
        {
            public void Configure(EntityTypeBuilder<BankAccountDbModel> builder)
            {
                builder.ToTable("bank_account");

                builder.Property(it => it.Id)
                    .HasColumnName("id");
                builder.HasKey(it => it.Id);

                builder.Property(it => it.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();

                builder.Property(it => it.Amount)
                    .HasColumnName("amount")
                    .IsRequired();

                builder.Property(it => it.CurrencyCode)
                    .HasColumnName("currency_code")
                    .IsRequired();

                builder.Property(it => it.OpeningDate)
                    .HasColumnName("opening_date")
                    .IsRequired();

                builder.Property(it => it.ClosingDate)
                    .HasColumnName("closing_date")
                    .IsRequired();

                builder.Property(it => it.IsClosed)
                    .HasColumnName("is_closed")
                    .IsRequired();
            }
        }
    }
}
