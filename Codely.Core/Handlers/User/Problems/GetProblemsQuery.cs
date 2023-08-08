using Codely.Core.Data;
using Codely.Core.Helpers;
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
        var submissions = await _context.Submissions
            .Where(x => x.UserId == _currentUserService.Id)
            .GroupBy(x => x.ProblemId)
            .Select(x => 
                new
                {
                    ProblemId = x.Key,
                    SubmissionStatus = x
                        .OrderByDescending(y => y.Created)
                        .Select(y => (SubmissionStatus?)y.SubmissionStatus)
                        .FirstOrDefault()
                })
            .ToListAsync(cancellationToken);

        var problemsData = await _context.Problems
            .Where(x => x.Status == ProblemStatus.Published)
            .Select(x =>
                new
                {
                    x.Id,
                    x.Title,
                    x.Description
                })
            .ToListAsync(cancellationToken);
        
        var problems = problemsData
            .Select(x =>
                new GetProblemsData
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ProblemSubmissionStatus = submissions
                        .Where(y => y.ProblemId == x.Id)
                        .Select(y => y.SubmissionStatus)
                        .FirstOrDefault()
                        .ToProblemSubmissionStatus()
                })
            .ToList();

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

    public  required string Title { get; init; }
    
    public required string Description { get; init; }

    public required ProblemSubmissionStatus ProblemSubmissionStatus { get; init; }
}