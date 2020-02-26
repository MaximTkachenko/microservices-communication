using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Portal.Services
{
    public interface ITokenAcquisitionService
    {
        Task AcquireTokenByAuthorizationCodeAsync(AuthorizationCodeReceivedContext context);
        Task<string> GetAccessTokenAsync();
    }
}
