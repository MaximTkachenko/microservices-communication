using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Users.WebApi.Db
{
    public class UsersDb: DbContext
    {
        public UsersDb(DbContextOptions<UsersDb> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

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
