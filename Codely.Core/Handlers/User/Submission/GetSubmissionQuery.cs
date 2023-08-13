using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Submission;

public sealed class GetSubmissionQuery : IRequestHandler<GetSubmissionsRequest,GetSubmissionsResponse>
{
    private readonly CodelyContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSubmissionQuery(CodelyContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<GetSubmissionsResponse> Handle(GetSubmissionsRequest request, CancellationToken cancellationToken)
    {
        var submissions = await _context.Submissions
            .Where(x => x.ProblemId == request.ProblemId)
            .Where(x => x.UserId == _currentUserService.Id)
            .Select(x =>
                new GetSubmissionData
                {
                    DateTime = x.Created,
                    Answer = x.Answer,
                    ProgrammingLanguage = x.ProgrammingLanguage,
                    ProblemSubmissionStatus = x.SubmissionStatus.ToProblemSubmissionStatus(),
                    NumberOfTestCases = x.SubmissionTestCases.Count,
                    NumberOfPassedTestCases = x.SubmissionTestCases
                        .Where(y => y.SubmissionTestCaseStatus == SubmissionTestCaseStatus.CorrectAnswer)
                        .Count()
                })
            .OrderByDescending(x => x.DateTime)
            .ToListAsync(cancellationToken);

        return new GetSubmissionsResponse
        {
            Submissions = submissions
        };
    }
}

public sealed class GetSubmissionsRequest : IRequest<GetSubmissionsResponse>
{
    public required int ProblemId { get; init; }
}

public sealed class GetSubmissionsResponse
{
    public List<GetSubmissionData> Submissions { get; init; } = new();
}

public sealed class GetSubmissionData
{
    public required DateTime DateTime { get; init; }

    public required string Answer { get; init; }
    
    public required ProgrammingLanguage ProgrammingLanguage { get; init; }
    
    public required ProblemSubmissionStatus ProblemSubmissionStatus { get; init; }

    public required int NumberOfTestCases { get; init; }

    public required int NumberOfPassedTestCases { get; init; }
}