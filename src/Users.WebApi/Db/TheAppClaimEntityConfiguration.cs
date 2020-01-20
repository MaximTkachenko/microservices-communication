using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Users.WebApi.Db
{
    public class TheAppClaimEntityConfiguration : IEntityTypeConfiguration<TheAppClaim>
    {
        public void Configure(EntityTypeBuilder<TheAppClaim> builder)
        {
            builder.ToTable("Claims")
                .HasKey(x => x.Id);
        }
    }
}
