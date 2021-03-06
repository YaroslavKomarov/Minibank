using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Minibank.Data.MoneyTransfersHistory
{
    public class MoneyTransferHistoryDbModel
    {
        public string Id { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }

    internal class Map : IEntityTypeConfiguration<MoneyTransferHistoryDbModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTransferHistoryDbModel> builder)
        {
            builder.ToTable("money_transfer_history");

            builder.Property(it => it.Id);

            builder.Property(it => it.Amount)
                .IsRequired();

            builder.Property(it => it.CurrencyCode)
                .IsRequired();

            builder.Property(it => it.FromAccountId)
                .IsRequired();

            builder.Property(it => it.ToAccountId)
                .IsRequired();
        }
    }
}
