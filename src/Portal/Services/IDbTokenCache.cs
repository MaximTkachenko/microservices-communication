using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Portal.Services
{
    public interface IDbTokenCache
    {
        void ConfigureCache(IConfidentialClientApplication application);
        Task RemoveCacheAsync();
    }
}
