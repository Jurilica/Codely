using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types.Enums;
using MediatR;

namespace Codely.Core.Handlers.User.Submission;

public sealed class SubmitAnswerCommand : IRequestHandler<SubmitAnswerRequest, SubmitAnswerResponse>
{
    private readonly CodelyContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly ITestCaseJobs _testCaseJobs;

    public SubmitAnswerCommand(CodelyContext context, ICurrentUserService currentUserService, ITestCaseJobs testCaseJobs)
    {
        _context = context;
        _currentUserService = currentUserService;
        _testCaseJobs = testCaseJobs;
    }
    
    public async Task<SubmitAnswerResponse> Handle(SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Answer, "Answer is empty");

        var submission = new Data.Entities.Submission
        {
            UserId = _currentUserService.Id,
            Answer = request.Answer,
            SubmissionStatus = SubmissionStatus.Created,
            ProgrammingLanguage = request.ProgrammingLanguage,
            ProblemId = request.ProblemId
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync(cancellationToken);

        _testCaseJobs.ExecuteTestCases(submission.Id);
        
        return new SubmitAnswerResponse
        {
            SubmissionId = submission.Id
        };
    }
}

public sealed class SubmitAnswerRequest : IRequest<SubmitAnswerResponse>
{
    public required int ProblemId { get; init; }
    
    public required string Answer { get; init; }
    
    public required ProgrammingLanguage ProgrammingLanguage { get; init; }
}

public sealed class SubmitAnswerResponse
{
    public required int SubmissionId { get; set; }
}