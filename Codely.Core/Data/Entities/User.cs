using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
}