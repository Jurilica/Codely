using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types;
using Codely.Core.Types.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.Account;

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

        var userWithSameEmailExists = await _context.Users
            .Where(x => x.Email == request.Email)
            .AnyAsync(cancellationToken);

        if (userWithSameEmailExists)
        {
            throw new CodelyException("User with same email already registered");
        }
        
        var userWithSameUsernameExists = await _context.Users
            .Where(x => x.Username == request.Username)
            .AnyAsync(cancellationToken);
        
        if (userWithSameUsernameExists)
        {
            throw new CodelyException("User with same username already registered");
        }

        var user = new Data.Entities.User
        {
            Email = request.Email,
            Username = request.Username,
            PasswordHash = PasswordHasher.Hash(request.Password),
            Role = Role.User,
            UserStatus = UserStatus.Active
        };

        _context.Users.Add(user);
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
    public required string Email { get; init; }

    public required string Username { get; init; } 

    public required string Password { get; init; }
}

public sealed class RegisterResponse
{
    public required string Token { get; init; }

    public required string RefreshToken { get; init; }
}