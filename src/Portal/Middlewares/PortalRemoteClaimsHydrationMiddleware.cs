using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
            IHttpClientFactory http)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var accessToken = string.Empty; //todo get from token cache
            var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                await _next(context);
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:5000/api/claims/{email}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
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
