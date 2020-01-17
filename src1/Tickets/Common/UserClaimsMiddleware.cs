using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Common
{
    public class UserClaimsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public UserClaimsMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.Value == _path)
            {
                var claims = context.User.Claims.Select(x => $"{x.Type} = {x.Value}").ToArray();

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(claims, new JsonSerializerOptions{WriteIndented = true}));
                return;
            }

            await _next(context);
        }
    }

    public static class UserClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserClaimsMiddleware(this IApplicationBuilder builder, string path)
        {
            return builder.UseMiddleware<UserClaimsMiddleware>(path);
        }
    }
}
