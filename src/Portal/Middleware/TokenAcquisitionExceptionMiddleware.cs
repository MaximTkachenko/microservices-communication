using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Portal.Middleware
{
    public class TokenAcquisitionExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenAcquisitionExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MsalUiRequiredException)
            {
                await context.ChallengeAsync();
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                await context.ChallengeAsync();
            }
        }
    }

    public static class TokenAcquisitionExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenAcquisitionException(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenAcquisitionExceptionMiddleware>();
        }
    }
}
