namespace Codely.Core.Data.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime? Archived { get; set; }
}