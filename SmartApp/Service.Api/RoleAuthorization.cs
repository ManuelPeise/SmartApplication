using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Enums;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RoleAuthorization : Attribute, IAuthorizationFilter
    {
        public UserRoleEnum RequiredRole { get; set; }
        public bool AllowAdmin { get; set; }
        public bool AllowMaintananceUser { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticated = false;

            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                var responseText = "You are not allowed to execute this action, please contact the administrator to get access!";

                context.Result = new ObjectResult(responseText)
                {
                    StatusCode = 401
                };
                
                return;
            }

            var roleClaimValue = user.Claims.FirstOrDefault(x => x.Type == "userRole")?.Value ?? null;

            if (!string.IsNullOrEmpty(roleClaimValue))
            {
                var role = (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), roleClaimValue);

                if (role == UserRoleEnum.MaintananceUser && AllowMaintananceUser == true)
                {
                    isAuthenticated = true;
                }
                else if (role == UserRoleEnum.Admin && AllowAdmin == true)
                {
                    isAuthenticated = true;
                }
                else
                {
                    isAuthenticated = RequiredRole == role;
                }
            }

            if (!isAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
