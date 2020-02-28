using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Portal.Db
{
    public class CachedToken
    {
        public string AccountId { get; set; }
        public byte[] Token { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class CachedTokenConfig : IEntityTypeConfiguration<CachedToken>
    {
        public void Configure(EntityTypeBuilder<CachedToken> builder)
        {
            builder.Property(p => p.AccountId).HasMaxLength(200);
            builder.HasKey(x => x.AccountId).IsClustered();
            builder.Property(x => x.Token).IsRequired();
            builder.Property(p => p.Timestamp).IsConcurrencyToken();
        }
    }
}
