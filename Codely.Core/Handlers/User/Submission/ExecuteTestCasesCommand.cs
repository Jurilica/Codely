using Codely.Core.Data;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Submission;

public sealed class ExecuteTestCasesCommand : IRequestHandler<ExecuteTestCasesRequest>
{
    private readonly CodelyContext _context;

    public ExecuteTestCasesCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task Handle(ExecuteTestCasesRequest request, CancellationToken cancellationToken)
    {
        var submissionData = await _context.Submissions
            .Where(x => x.Id == request.SubmissionId)
            .Select(x =>
                new
                {
                    Submission = x,
                    x.Problem.TestCases,
                    x.ProgrammingLanguage
                })
            .FirstOrDefaultAsync(cancellationToken);

        if (submissionData is null)
        {
            throw new CodelyException("Submission not found");
        }

        var allTestCasesPassed = true;

        foreach (var testCase in submissionData.TestCases)
        {
          
        }

        submissionData.Submission.SubmissionStatus = allTestCasesPassed 
            ? SubmissionStatus.Succeeded 
            : SubmissionStatus.Failed;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public sealed class ExecuteTestCasesRequest : IRequest
{
    public required int SubmissionId { get; init; }
}