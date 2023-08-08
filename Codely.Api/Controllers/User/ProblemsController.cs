using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.User.Problems;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.User;

[Route("user/problems")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.User)]
[RoleAuthorize(Role.User)]
public class ProblemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProblemsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<GetProblemsResponse> GetProblems()
    {
       return await _mediator.Send(new GetProblemsRequest());
    }
}