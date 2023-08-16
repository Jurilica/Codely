using Codely.Core.Data;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Admin.Problems;

public sealed class GetProblemsQuery : IRequestHandler<GetProblemsRequest, GetProblemsResponse>
{
    private readonly CodelyContext _context;

    public GetProblemsQuery(CodelyContext context)
    {
        _context = context;
    }

    public async Task<GetProblemsResponse> Handle(GetProblemsRequest request, CancellationToken cancellationToken)
    {
        var problems = await _context.Problems
            .Select(x =>
                new GetProblemsData
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Difficulty = x.Difficulty,
                    ProblemStatus = x.Status
                })
            .ToListAsync(cancellationToken);

        return new GetProblemsResponse
        {
            Problems = problems
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

    public required string Title { get; init; }
    
    public required string Description { get; init; }
    
    public required ProblemDifficulty Difficulty { get; init; }

    public required ProblemStatus ProblemStatus { get; init; }
}

