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
    private const string LineEnding = "\\n";
    
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

        // in batch file line ending represents new argument
        // line endings are replaced so input can be only one parameter
        testCaseInputsWithDelimiter = testCaseInputsWithDelimiter.ReplaceLineEndings(LineEnding);
        
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