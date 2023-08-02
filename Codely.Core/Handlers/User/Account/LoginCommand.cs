using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Account;

public sealed class LoginCommand : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly CodelyContext _context;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public LoginCommand(CodelyContext context, IJwtTokenProvider jwtTokenProvider)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Email, "Empty email")
            .IsEmpty(request.Password, "Empty password");

        var user = await _context.Users
            .Where(x => x.Email == request.Email)
            .SingleOrDefaultAsync(cancellationToken);

        var isPasswordValid = PasswordHasher.Verify(user?.PasswordHash, request.Password);

        if (!isPasswordValid)
        {
            throw new CodelyException("Wrong email or password");
        }

        var refreshToken = _jwtTokenProvider.CreateRefreshToken(user!.Id);

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var token = _jwtTokenProvider.Generate(user!.Id, user!.Username, user!.Email, user!.Role);
        
        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken.Token
        };
    }
}

public sealed class LoginRequest : IRequest<LoginResponse>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}

public sealed class LoginResponse
{
    public required string Token { get; init; }
    
    public required string RefreshToken { get; init; }
}