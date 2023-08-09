using System.ComponentModel;
using CliWrap;
using CliWrap.Buffered;
using Codely.Core.Types.Enums;

namespace Codely.Core.Services;

public interface ICodeExecutionService
{
    Task<string> ExecuteCode(string sourceCode, ProgrammingLanguage programmingLanguage,
        string testCaseInputsWithDelimiter, CancellationToken cancellationToken);
}

public sealed class CodeExecutionService : ICodeExecutionService
{
    public async Task<string> ExecuteCode(string sourceCode, ProgrammingLanguage programmingLanguage,
        string testCaseInputsWithDelimiter, CancellationToken cancellationToken)
    {
        var fileName = Guid.NewGuid();

        var extension = programmingLanguage switch
        {
            ProgrammingLanguage.Python => ".py",
            ProgrammingLanguage.JavaScript => ".js",
            ProgrammingLanguage.Cpp => ".cpp",
            _ => throw new InvalidEnumArgumentException()
        };
        
        var path = $@"c:\Test\{fileName}{extension}";

        await File.WriteAllTextAsync(path, sourceCode, cancellationToken);
        
        var args = new List<string>
        {
            path, testCaseInputsWithDelimiter
        };

        var result = await Cli.Wrap(@"..\docker\execute.bat")
            .WithArguments(args)
            .ExecuteBufferedAsync(cancellationToken);

        return result.StandardOutput;
    }
}