using System.Linq;
using System.Security.Claims;

namespace Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetEmail(this ClaimsPrincipal cp, out string email)
        {
            email = cp.Identities.First().Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
            return !string.IsNullOrEmpty(email);
        }

        public static bool TryFindClaim(this ClaimsPrincipal cp, string type, string value)
        {
            return cp.Identities.First().Claims.Any(x => x.Type == type && x.Value == value);
        }
    }
}
