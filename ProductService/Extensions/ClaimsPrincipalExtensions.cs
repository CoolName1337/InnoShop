using System.Security.Claims;

namespace ProductService.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal user, out int userId)
        {
            var userIdClaimString = user.FindFirst("userId")?.Value;
            if (!string.IsNullOrEmpty(userIdClaimString) && int.TryParse(userIdClaimString, out int userIdInt))
            {
                userId = userIdInt;
                return true;
            }
            userId = 0;
            return false;
        }
    }
}
