using System.Diagnostics.CodeAnalysis;
using Codely.Core.Types;

namespace Codely.Core.Helpers;

public static class GuardExtensions
{
    public static Guard IsEmpty(this Guard guard, [NotNull] string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new CodelyException(message);
        }

        return guard;
    }

    public static Guard IsToShort(this Guard guard, [NotNull] string? value, int minLength, string message)
    {
        if (value.Length < minLength)
        {
            throw new CodelyException(message);
        }

        return guard;
    }
}

public class Guard
{
    public static Guard Against => new();
}
