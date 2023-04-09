using System.Security.Claims;
using Codely.Core.Services;
using Codely.Core.Types;
using Codely.Core.Types.Enums;

namespace Codely.Api.Authentication;

public class CurrentUserResolver : ICurrentUserService
{
    public CurrentUserResolver(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext!.User.Identity as ClaimsIdentity;
        
        if (identity is null)
        {
            throw new CodelyException(nameof(identity));
        }

        Id = int.Parse(identity.FindFirst(UserClaimType.UserId)?.Value!);
        Username = identity.FindFirst(UserClaimType.Name)?.Value!;
        Email = identity.FindFirst(UserClaimType.Email)?.Value!;
        Role = Enum.Parse<Role>(identity.FindFirst(UserClaimType.Role)?.Value!);
    }
    
    public int Id { get; }

    public string Username { get; }

    public string Email { get; }
    
    public Role Role { get; }
}