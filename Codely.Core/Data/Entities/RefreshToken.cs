using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;

    public DateTime ValidUntil { get; set; }

    public DateTime? UsedOn { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}