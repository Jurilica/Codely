namespace Codely.Core.Services;

public interface ISystemTime
{
    public DateTime Now { get; }
}

public class SystemTime : ISystemTime
{
    public DateTime Now => DateTime.UtcNow;
}