using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Users.WebApi.Middlewares
{
    public class LocalClaimsHydrationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalClaimsHydrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                return;
            }

            var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (!string.IsNullOrEmpty(email))
            {
                //todo read from database + caching
                context.User.Identities.First().AddClaim(new Claim("x-userid", Math.Abs(email.GetHashCode()).ToString(), "integer", "theapp"));
            }

            await _next(context);
        }
    }

    public static class LocalClaimsHydrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseLocalClaimsHydrationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalClaimsHydrationMiddleware>();
        }
    }
}
