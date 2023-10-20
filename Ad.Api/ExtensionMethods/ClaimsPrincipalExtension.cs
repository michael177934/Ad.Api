using Ad.Core.Constants;
using System.Security.Claims;

namespace Ad.API.ExtensionMethods
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static Guid GetTenantId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return Guid.Parse(principal.FindFirstValue(CustomClaimTypes.TenantId));
        }
    }
}