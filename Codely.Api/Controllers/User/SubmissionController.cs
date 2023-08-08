using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.User.Submission;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.User;

[Route("user/submission")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.User)]
[RoleAuthorize(Role.User)]
public class SubmissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubmissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{problemId}")]
    public async Task<GetSubmissionsResponse> GetSubmissions(int problemId)
    {
        return await _mediator.Send(new GetSubmissionsRequest
        {
            ProblemId = problemId
        });
    }

    [HttpPost("submit")]
    public async Task Submit(SubmitAnswerRequest request)
    {
        await _mediator.Send(request);
    }
}