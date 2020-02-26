using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;

namespace Portal.Services
{
    public class FromCacheAccessTokenGetter : IAccessTokenGetter
    {
        private readonly ITokenAcquisitionService _tokenService;

        public FromCacheAccessTokenGetter(ITokenAcquisitionService tokenService)
        {
            _tokenService = tokenService;
        }

        public Task<string> GetTokenAsync(HttpContext context) =>
            _tokenService.GetAccessTokenAsync();
    }
}
