using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common
{
    public interface IAccessTokenGetter
    {
        Task<string> GetTokenAsync(HttpContext context);
    }
}
