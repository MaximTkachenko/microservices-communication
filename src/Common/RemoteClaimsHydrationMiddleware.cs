using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Common.UsersApiModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public class RemoteClaimsHydrationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _usersApiBaseUrl;

        public RemoteClaimsHydrationMiddleware(RequestDelegate next,
            IConfiguration config)
        {
            _next = next;
            _usersApiBaseUrl = config.GetValue<string>("Services:UsersApiUrl");
        }

        public async Task InvokeAsync(HttpContext context,
            IHttpClientFactory http,
            IAccessTokenGetter accessTokenGetter)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            if (!context.User.TryGetEmail(out var email))
            {
                await _next(context);
                return;
            }

            var accessToken = await accessTokenGetter.GetTokenAsync(context);

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_usersApiBaseUrl}/api/users/{email}/claims");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await http.CreateClient().SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                await _next(context);
                return;
            }

            IEnumerable<ApiClaim> claims;
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                claims = await JsonSerializer.DeserializeAsync<ApiClaim[]>(responseStream);
            }

            foreach (var claim in claims)
            {
                context.User.Identities.First().AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            await _next(context);
        }
    }

    public static class RemoteClaimsHydrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRemoteClaimsHydration(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RemoteClaimsHydrationMiddleware>();
        }
    }
}
