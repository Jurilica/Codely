using Codely.Core.Handlers.User.Submission;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.User;

[Route("user/submission")]
[ApiController]
public class SubmissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubmissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("submit")]
    public async Task Submit(SubmitAnswerRequest request)
    {
        await _mediator.Send(request);
    }
}