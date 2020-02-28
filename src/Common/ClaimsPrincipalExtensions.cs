using System.Linq;
using System.Security.Claims;

namespace Common
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetEmail(this ClaimsPrincipal claimsPrincipal, out string email)
        {
            email = claimsPrincipal.FindFirstValue("preferred_username");
            return !string.IsNullOrEmpty(email);
        }

        public static bool TryFindClaim(this ClaimsPrincipal cp, string type, string value)
        {
            return cp.Identities.First().Claims.Any(x => x.Type == type && x.Value == value);
        }

        public static string GetObjectId(this ClaimsPrincipal claimsPrincipal)
        {
            string userObjectId = claimsPrincipal.FindFirstValue("oid");
            if (string.IsNullOrEmpty(userObjectId))
            {
                userObjectId = claimsPrincipal.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier");
            }
            return userObjectId;
        }

        public static string GetTenantId(this ClaimsPrincipal claimsPrincipal)
        {
            string tenantId = claimsPrincipal.FindFirstValue("tid");
            if (string.IsNullOrEmpty(tenantId))
            {
                return claimsPrincipal.FindFirstValue("http://schemas.microsoft.com/identity/claims/tenantid");
            }

            return tenantId;
        }

        public static string GetMsalAccountId(this ClaimsPrincipal claimsPrincipal)
        {
            string userObjectId = claimsPrincipal.GetObjectId();
            string tenantId = claimsPrincipal.GetTenantId();

            if (string.IsNullOrEmpty(userObjectId) || string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            return $"{userObjectId}.{tenantId}";
        }
    }
}
