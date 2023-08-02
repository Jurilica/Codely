using Codely.Core.Data;
using Codely.Core.Services;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Problems;

public sealed class GetProblemsQuery : IRequestHandler<GetProblemsRequest, GetProblemsResponse>
{
    private readonly CodelyContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProblemsQuery(CodelyContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<GetProblemsResponse> Handle(GetProblemsRequest request, CancellationToken cancellationToken)
    {
        var solvedProblems = await _context.Submissions
            .Where(x => x.UserId == _currentUserService.Id)
            .Where(x => x.SubmissionStatus == SubmissionStatus.Succeeded)
            .Select(x => x.ProblemId)
            .ToListAsync(cancellationToken);

        var problems = await _context.Problems
            .Where(x => x.Status == ProblemStatus.Published)
            .Select(x =>
                new GetProblemsData
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsSolved = solvedProblems.Contains(x.Id),
                    Examples = x.Examples
                        .Select(y =>
                            new ExampleData
                            {
                                Input = y.Input,
                                Output = y.Output,
                                Explanation = y.Explanation
                            })
                        .ToList()
                })
            .ToListAsync(cancellationToken);

        return new GetProblemsResponse
        {
            Problems= problems
        };
    }
}

public sealed class GetProblemsRequest : IRequest<GetProblemsResponse>
{
}

public sealed class GetProblemsResponse
{
    public required List<GetProblemsData> Problems { get; init; }
}

public sealed class GetProblemsData
{
    public required int Id { get; init; }

    public  required string Title { get; init; }
    
    public required string Description { get; init; }

    public required bool IsSolved { get; init; }

    public required List<ExampleData> Examples { get; init; }
}

public sealed class ExampleData
{
    public required string Input { get; init; }

    public required string Output { get; init; }

    public required string Explanation { get; init; } 
}