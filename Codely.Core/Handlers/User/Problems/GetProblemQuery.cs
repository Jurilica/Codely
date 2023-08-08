using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Problems;

public sealed class GetProblemQuery : IRequestHandler<GetProblemRequest, GetProblemResponse>
{
    private readonly CodelyContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProblemQuery(CodelyContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<GetProblemResponse> Handle(GetProblemRequest request, CancellationToken cancellationToken)
    {
        var lastSubmissionStatus = await _context.Submissions
            .Where(x => x.ProblemId == request.ProblemId)
            .Where(x => x.UserId == _currentUserService.Id)
            .OrderByDescending(x => x.Created)
            .Select(x => (SubmissionStatus?)x.SubmissionStatus)
            .FirstOrDefaultAsync(cancellationToken);

        var problem = await _context.Problems
            .Where(x => x.Id == request.ProblemId)
            .Where(x => x.Status == ProblemStatus.Published)
            .Select(x =>
                new GetProblemData
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ProblemSubmissionStatus = lastSubmissionStatus.ToProblemSubmissionStatus(),
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
            .FirstOrDefaultAsync(cancellationToken);

        if (problem is null)
        {
            throw new CodelyException("Problem not found");
        }

        return new GetProblemResponse
        {
            Problem = problem
        };
    }
}

public sealed class GetProblemRequest : IRequest<GetProblemResponse>
{
    public required int ProblemId { get; init; }
}

public sealed class GetProblemResponse
{
    public required GetProblemData Problem { get; init; }
}

public sealed class GetProblemData
{
    public required int Id { get; init; }

    public  required string Title { get; init; }
    
    public required string Description { get; init; }

    public required ProblemSubmissionStatus ProblemSubmissionStatus { get; init; }

    public required List<ExampleData> Examples { get; init; }
}

public sealed class ExampleData
{
    public required string Input { get; init; }

    public required string Output { get; init; }

    public required string Explanation { get; init; } 
}