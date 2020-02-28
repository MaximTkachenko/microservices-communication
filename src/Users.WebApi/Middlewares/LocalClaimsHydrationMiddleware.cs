using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Users.WebApi.Db;
using Users.WebApi.Services;

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
            UsersDb db,
            IClaimsService claimsService)
        {
            if (!context.User.Identity.IsAuthenticated
                || !context.User.TryGetEmail(out var email))
            {
                await _next(context);
                return;
            }

            var identity = context.User.Identities.First();
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                await _next(context);
                return;
            }

            foreach (var claim in await claimsService.GetClaimsAsync(user.Id))
            {
                identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue, claim.ClaimValueType, "theapp"));
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
