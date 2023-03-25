namespace Codely.Core.Services;

public interface ICurrentUserService
{
    public int Id { get; }

    public string Username { get; }

    public string Email { get; }
}