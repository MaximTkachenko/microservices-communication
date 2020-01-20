using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Users.WebApi.Db;

namespace Users.WebApi.Middlewares
{
    public class LocalClaimsHydrationMiddleware
    {
        private readonly RequestDelegate _next;

        public LocalClaimsHydrationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context,
            UsersDb db)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            if (context.User.TryGetEmail(out var email))
            {
                var identity = context.User.Identities.First();
                var user = await db.Users.Include(x => x.Claims).FirstAsync(x => x.Email == email);
                foreach (var claim in user.Claims)
                {
                    identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue, claim.ClaimValueType, "theapp"));
                }
            }

            await _next(context);
        }
    }

    public static class LocalClaimsHydrationMiddlewareExtensions
    {
        public static IApplicationBuilder UseLocalClaimsHydration(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LocalClaimsHydrationMiddleware>();
        }
    }
}
