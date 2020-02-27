using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Portal.Services
{
    public class DbTokenCache : IDbTokenCache
    {
        public void ConfigureCache(IConfidentialClientApplication application)
        {
            
        }

        public Task RemoveCacheAsync()
        {
            //todo
            return Task.CompletedTask;
        }
    }
}
