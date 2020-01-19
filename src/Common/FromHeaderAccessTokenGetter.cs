using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common
{
    public class FromHeaderAccessTokenGetter: IAccessTokenGetter
    {
        public Task<string> GetTokenAsync(HttpContext context)
        {
            var accessToken = context.Request.Headers["Authorization"].First().Split(' ')[1];
            return Task.FromResult(accessToken);
        }
    }
}
