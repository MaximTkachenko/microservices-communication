using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Identity.Client;

namespace Portal.Services
{
    /// <summary>
    /// https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2/tree/master/2-WebApp-graph-user/2-2-TokenCache
    /// </summary>
    public class TokenAcquisitionService : ITokenAcquisitionService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly Lazy<IConfidentialClientApplication> _application;
        private readonly IDbTokenCache _cache;

        public TokenAcquisitionService(IHttpContextAccessor httpContext,
            IDbTokenCache cache)
        {
            _httpContext = httpContext;
            _cache = cache;
            _application = new Lazy<IConfidentialClientApplication>(BuildConfidentialClientApplication);
        }

        public async Task AcquireTokenByAuthorizationCodeAsync(AuthorizationCodeReceivedContext context)
        {
            // The cache will need the claims from the ID token.
            // If they are not yet in the HttpContext.User's claims, add them here.
            if (!context.HttpContext.User.Claims.Any())
            {
                context.HttpContext.User.Identities.First().AddClaims(context.Principal.Claims);
            }

            var result = await _application.Value.AcquireTokenByAuthorizationCode(new[] { "api://theapp.api/UsersAndClaims", "api://theapp.api/Tickets" }, context.ProtocolMessage.Code)
                .ExecuteAsync();

            context.HandleCodeRedemption(result.AccessToken, result.IdToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var accountId = _httpContext.HttpContext.User.GetMsalAccountId();
            IAccount account = await _application.Value.GetAccountAsync(accountId);
            var result = await _application.Value.AcquireTokenSilent(new[] {"api://theapp.api/UsersAndClaims", "api://theapp.api/Tickets"}, account)
                .ExecuteAsync();
            return result.AccessToken;
        }

        private IConfidentialClientApplication BuildConfidentialClientApplication()
        {
            //use MSAL for v2.0
            //https://docs.microsoft.com/bs-latn-ba/azure/active-directory/develop/msal-net-instantiate-confidential-client-config-options
            //https://joonasw.net/view/azure-ad-v2-and-msal-from-dev-pov
            //https://securecloud.blog/2019/05/22/azure-api-management-jwt-validation-for-multiple-azure-ad-partner-registrations/
            //https://thomaslevesque.com/2018/12/24/multitenant-azure-ad-issuer-validation-in-asp-net-core/
            //https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant

            var request = _httpContext.HttpContext.Request;
            var currentUri = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path);
            var co = new ConfidentialClientApplicationOptions
            {
                Instance = "https://login.microsoftonline.com/",
                TenantId = "common",
                ClientId = "b021b14e-1671-4fe6-b7cc-0a67a248543f",
                ClientSecret = "Ushs_5=SuttP50l7ZEovc?l]1[H3Z9k1",
                RedirectUri = currentUri
            };
            
            var app = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(co)
                .Build();

            _cache.ConfigureCache(app);

            return app;
        }
    }
}
