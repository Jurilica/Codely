using Codely.Core.Services;
using Codely.Core.Types.Enums;

namespace Codely.Worker.Resolver;

public sealed class UserResolver : ICurrentUserService
{
    public UserResolver()
    {
        Id = 1;
        Username = "Worker";
        Email = "Worker";
        Role = Role.User;
    }
    
    public int Id { get; }

    public string Username { get; }

    public string Email { get; }
    
    public Role Role { get; }
}