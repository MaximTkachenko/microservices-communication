using Microsoft.EntityFrameworkCore;

namespace Users.WebApi.Db
{
    public class UsersDb: DbContext
    {
        public UsersDb(DbContextOptions<UsersDb> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<TheAppClaim> Claims { get; set; }
    }
}
