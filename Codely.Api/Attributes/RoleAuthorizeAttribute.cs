using Codely.Core.Services;
using Codely.Core.Types.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Codely.Api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role _role;

    public RoleAuthorizeAttribute(Role role)
    {
        _role = role;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();
        if (user.Role != _role)
        {
            context.Result = new ForbidResult();
        }
    }
}