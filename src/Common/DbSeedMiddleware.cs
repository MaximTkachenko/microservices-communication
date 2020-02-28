using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Common
{
    public class DbSeedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _path;

        public DbSeedMiddleware(RequestDelegate next, string path)
        {
            _next = next;
            _path = path;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //todo check db existence, create if needed
            //todo run initial migration script
            //todo run seed script

            await _next(context);
        }
    }

    public static class UserDbSeedMiddlewareExtensions
    {
        public static IApplicationBuilder UseDbSeed(this IApplicationBuilder builder, string path)
        {
            return builder.UseMiddleware<DbSeedMiddleware>(path);
        }
    }
}
