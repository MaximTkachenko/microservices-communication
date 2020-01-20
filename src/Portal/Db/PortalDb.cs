using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Portal.Db
{
    public class PortalDb : DbContext
    {
        public PortalDb(DbContextOptions<PortalDb> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
