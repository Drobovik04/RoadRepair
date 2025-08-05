using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RoadRepair.API.Attributes
{
    public class BlockedUserFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isBlockedClaim = context.HttpContext.User.FindFirst("isBlocked")?.Value;

            if (bool.TryParse(isBlockedClaim, out bool isBlocked) && isBlocked)
            {
                context.Result = new ForbidResult(); // 🔐 Запретить доступ
            }
        }
    }

}
