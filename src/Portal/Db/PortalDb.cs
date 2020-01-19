using Microsoft.EntityFrameworkCore;

namespace Portal.Db
{
    public class PortalDb : DbContext
    {
        public PortalDb(DbContextOptions<PortalDb> options) : base(options)
        { }
    }
}
