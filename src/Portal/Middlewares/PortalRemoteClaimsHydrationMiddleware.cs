using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using IHttpClientFactory = System.Net.Http.IHttpClientFactory;

namespace Portal.Middlewares
{
    public class PortalRemoteClaimsHydrationMiddleware
    {
        private readonly RequestDelegate _next;

        public PortalRemoteClaimsHydrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            IHttpClientFactory http,
            IConfiguration c)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }



            var cp = typeof(TokenCache).GetField("_tokenCacheDictionary", BindingFlags.Instance | BindingFlags.NonPublic);
            var tokens = cp.GetValue(context.RequestServices.GetService<TokenCache>());

            var opts = new AzureADOptions();
            c.Bind("AzureAd", opts);
            //todo why cache isn't working?!!!
            var credential = new ClientCredential(opts.ClientId, opts.ClientSecret);
            var authContext = new AuthenticationContext(opts.Instance.Replace("common", context.User.Claims.First(x => x.Type == "http://schemas.microsoft.com/identity/claims/tenantid").Value), context.RequestServices.GetService<TokenCache>());
            var accessToken = await authContext.AcquireTokenSilentAsync("api://theapp.api", credential, 
                new UserIdentifier(context.User.Claims.First(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value, UserIdentifierType.UniqueId));
            var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                await _next(context);
                return;
            }

            //todo put url to claims api into config
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5000/api/claims/{email}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
            var response = await http.CreateClient().SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await _next(context);
                return;
            }

            IEnumerable<KeyValuePair<string, string>> claims;
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                claims = await JsonSerializer.DeserializeAsync<IEnumerable<KeyValuePair<string, string>>>(responseStream);
            }

            foreach (var claim in claims)
            {
                context.User.Identities.First().AddClaim(new Claim(claim.Key, claim.Value));
            }

            await _next(context);
        }
    }

    public static class PortalRemoteClaimsHydrationMiddlewareExtensions
    {
        public static IApplicationBuilder UsePortalRemoteClaimsHydrationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PortalRemoteClaimsHydrationMiddleware>();
        }
    }
}
