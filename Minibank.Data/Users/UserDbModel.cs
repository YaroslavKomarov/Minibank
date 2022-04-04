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
                builder.ToTable("users");

                builder.Property(it => it.Id)
                    .HasColumnName("id");

                builder.Property(it => it.Login)
                    .HasColumnName("login")
                    .IsRequired()
                    .HasMaxLength(29);

                builder.Property(it => it.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(256);
            }
        }
    }
}
