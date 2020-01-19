using System.Linq;
using System.Security.Claims;

namespace Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetEmail(this ClaimsPrincipal cp, out string email)
        {
            email = cp.Identities.First().Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return !string.IsNullOrEmpty(email);
        }
    }
}
