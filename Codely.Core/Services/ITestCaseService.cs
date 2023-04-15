using Codely.Core.Handlers.User.Submission;
using MediatR;

namespace Codely.Core.Services;

public interface ITestCaseService
{
    Task ExecuteTestCases(int submissionId);
}

public sealed class TestCaseService : ITestCaseService
{
    private readonly IMediator _mediator;

    public TestCaseService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task ExecuteTestCases(int submissionId)
    {
        var request = new ExecuteTestCasesRequest
        {
            SubmissionId = submissionId
        };

        await _mediator.Send(request);
    }
}