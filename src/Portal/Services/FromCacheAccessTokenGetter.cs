using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Portal.Services
{
    public class FromCacheAccessTokenGetter : IAccessTokenGetter
    {
        private readonly IConfiguration _config;

        public FromCacheAccessTokenGetter(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetTokenAsync(HttpContext context)
        {
            var opts = new AzureADOptions();
            _config.Bind("AzureAd", opts);
            var credential = new ClientCredential(opts.ClientId, opts.ClientSecret);

            //TokenCache doesn't respect common authority and call refresh every time
            //so here we read tenantId from claims
            var tenantId = context.User.Claims.First(x => x.Type == "http://schemas.microsoft.com/identity/claims/tenantid").Value;
            var authority = opts.Instance.Replace("/common", $"/{tenantId}");

            var authContext = new AuthenticationContext(authority, context.RequestServices.GetService<TokenCache>());
            var accessToken = await authContext.AcquireTokenSilentAsync("api://theapp.api", credential,
                new UserIdentifier(context.User.Claims.First(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value, UserIdentifierType.UniqueId));
            return accessToken.AccessToken;
        }
    }
}
