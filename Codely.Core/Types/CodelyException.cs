namespace Codely.Core.Types;

public class CodelyException : Exception
{
    public CodelyException(string message)
        : base(message)
    {
    }
}