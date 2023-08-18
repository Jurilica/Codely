using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.User.Leaderboard;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.User;

[Route("user/leaderboard")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.User)]
[RoleAuthorize(Role.User)]
public class LeaderboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaderboardController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<GetLeaderboardResponse> GetSubmissions()
    {
        return await _mediator.Send(new GetLeaderboardRequest());
    }
}