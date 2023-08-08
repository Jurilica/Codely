using Codely.Core.Data.Entities.Base;

namespace Codely.Core.Data.Entities;

public sealed class RefreshToken : BaseEntity
{
    public required string Token { get; set; }

    public required DateTime ValidUntil { get; set; }

    public DateTime? UsedOn { get; set; }

    public required int UserId { get; set; }

    public User User { get; set; } = null!;
}