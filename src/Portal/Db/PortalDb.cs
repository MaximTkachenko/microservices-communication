using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Portal.Db
{
    public class PortalDb : DbContext
    {
        public PortalDb(DbContextOptions<PortalDb> options) : base(options)
        { }

        public DbSet<CachedToken> CachedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //disable "ON DELETE CASCADE"
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
