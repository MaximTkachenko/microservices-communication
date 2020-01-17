using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Portal.Db;

namespace Portal.Services
{
    public class DbTokenCache : TokenCache
    {
        private readonly PortalDb _db;

        public DbTokenCache(PortalDb db)
        {
            _db = db;
        }
    }
}
