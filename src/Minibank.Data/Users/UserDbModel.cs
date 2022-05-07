using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Minibank.Data.Users
{
    public class UserDbModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        internal class Map : IEntityTypeConfiguration<UserDbModel>
        {
            public void Configure(EntityTypeBuilder<UserDbModel> builder)
            {
                builder.ToTable("user");

                builder.Property(it => it.Id);

                builder.Property(it => it.Login)
                    .IsRequired()
                    .HasMaxLength(29);

                builder.Property(it => it.Email)
                    .IsRequired()
                    .HasMaxLength(256);
            }
        }
    }
}
