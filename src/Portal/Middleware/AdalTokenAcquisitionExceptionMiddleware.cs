using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Portal.Middleware
{
    public class AdalTokenAcquisitionExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AdalTokenAcquisitionExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(AdalSilentTokenAcquisitionException)
            {
                await context.ChallengeAsync();
            }
        }
    }

    public static class AdalTokenAcquisitionExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAdalTokenAcquisitionException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AdalTokenAcquisitionExceptionMiddleware>();
        }
    }
}
