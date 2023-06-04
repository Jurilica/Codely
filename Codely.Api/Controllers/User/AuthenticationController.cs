using Codely.Core.Handlers.User.Account;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Codely.Api.Controllers.User;

[Route("user/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        return await _mediator.Send(request);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
    {
        return await _mediator.Send(request);
    }
}