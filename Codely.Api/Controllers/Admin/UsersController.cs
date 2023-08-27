using Codely.Api.Attributes;
using Codely.Api.Constants;
using Codely.Core.Handlers.Admin.Users;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.Admin;

[Route("admin/users")]
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerConstants.Admin)]
[RoleAuthorize(Role.Admin)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<GetUsersResponse> GetUsers()
    {
        return await _mediator.Send(new GetUsersRequest());
    }
    
    [HttpPut("{username}/ban")]
    public async Task<BanUserResponse> BanUser(string username)
    {
        return await _mediator.Send(new BanUserRequest
        {
            Username = username
        });
    }
    
    [HttpPut("{username}/unban")]
    public async Task<UnbanUserResponse> UnbanUser(string username)
    {
        return await _mediator.Send(new UnbanUserRequest
        {
            Username = username
        });
    }
}