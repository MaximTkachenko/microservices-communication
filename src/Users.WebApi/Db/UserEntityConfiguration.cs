using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.WebApi.Db
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email).HasMaxLength(300).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Name).HasMaxLength(500).IsRequired();
            builder.Property(x => x.CustomerName).HasMaxLength(300).IsRequired();
        }
    }
}
