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

        var problemsData = await _context.Problems
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
            ProblemsData = problemsData
        };
    }
}

public sealed class GetProblemsRequest : IRequest<GetProblemsResponse>
{
}

public sealed class GetProblemsResponse
{
    public List<GetProblemsData> ProblemsData { get; init; } = new();
}

public sealed class GetProblemsData
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;
    
    public string Description { get; init; } = string.Empty;

    public bool IsSolved { get; init; }

    public List<ExampleData> Examples { get; init; } = new();
}

public sealed class ExampleData
{
    public string Input { get; set; } = string.Empty;

    public string Output { get; set; } = string.Empty;

    public string Explanation { get; set; } = string.Empty;
}