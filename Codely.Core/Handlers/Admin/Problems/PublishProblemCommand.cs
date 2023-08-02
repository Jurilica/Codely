using Codely.Core.Data;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class PublishProblemCommand : IRequestHandler<PublishProblemRequest, PublishProblemResponse>
{
    private readonly CodelyContext _context;

    public PublishProblemCommand(CodelyContext context)
    {
        _context = context;
    }
    
    public async Task<PublishProblemResponse> Handle(PublishProblemRequest request, CancellationToken cancellationToken)
    {
        var problemData = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .Select(x =>
                new
                {
                    TestCasesExists = x.TestCases.Any(),
                    ExamplesExists = x.Examples.Any(),
                    Problem = x
                })
            .FirstAsync(cancellationToken);

        if (!problemData.TestCasesExists)
        {
            throw new CodelyException("Problem can't be published without any test case");
        }
        
        if (!problemData.ExamplesExists)
        {
            throw new CodelyException("Problem can't be published without any example");
        }

        problemData.Problem.Status = ProblemStatus.Published;
        await _context.SaveChangesAsync(cancellationToken);
        
        return new PublishProblemResponse();
    }
}

public sealed class PublishProblemRequest : IRequest<PublishProblemResponse>
{
    public required int ProblemId { get; init; }
}

public sealed class PublishProblemResponse
{
}