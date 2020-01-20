using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Users.WebApi.Db
{
    public class UsersDb: DbContext
    {
        public UsersDb(DbContextOptions<UsersDb> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<TheAppClaim> Claims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
