using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types.Enums;
using MediatR;

namespace Codely.Core.Handlers.User.Account;

public sealed class RegisterCommand : IRequestHandler<RegisterRequest, RegisterResponse>
{
    private readonly CodelyContext _context;
    private readonly IJwtTokenProvider _jwtTokenProvider;

    public RegisterCommand(CodelyContext context, IJwtTokenProvider jwtTokenProvider)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
    }
    
    public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.Email, "Empty email")
            .IsEmpty(request.Password, "Empty password")
            .IsToShort(request.Password, 5, "Password is to short");
        
        var user = new Data.Entities.User
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = Role.User
        };

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var refreshToken = _jwtTokenProvider.CreateRefreshToken(user.Id);

        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var token = _jwtTokenProvider.Generate(user.Id, user.Username, user.Email, user.Role);

        return new RegisterResponse
        {
            Token = token,
            RefreshToken = refreshToken.Token
        };
    }
}

public sealed class RegisterRequest : IRequest<RegisterResponse>
{
    public string Email { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

public sealed class RegisterResponse
{
    public string Token { get; init; } = string.Empty;
    
    public string RefreshToken { get; init; } = string.Empty;
}