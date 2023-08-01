using Codely.Api.Constants;
using Codely.Core.Handlers.Admin.Problems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.Admin;

[Route("user/problems")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.Admin)]
public class ProblemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProblemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<CreateProblemResponse> CreateProblem(CreateProblemRequest request)
    {
        return await _mediator.Send(request);
    }
    
    [HttpGet]
    public async Task<GetProblemsResponse> GetProblems()
    {
        return await _mediator.Send(new GetProblemsRequest());
    }
}