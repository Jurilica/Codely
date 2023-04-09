using Codely.Core.Data.Entities.Base;
using Codely.Core.Types.Enums;

namespace Codely.Core.Data.Entities;

public sealed class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;

    public Role Role { get; set; }
}