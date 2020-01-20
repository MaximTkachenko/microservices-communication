using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.WebApi.Db
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users")
                .HasKey(x => x.Id);

            builder.Metadata.FindNavigation(nameof(User.Claims))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
