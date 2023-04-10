using Codely.Core.Data;
using Codely.Core.Helpers;
using Codely.Core.Services;
using Codely.Core.Types;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codely.Core.Handlers.User.Account;

public sealed class RefreshTokenCommand : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    private readonly CodelyContext _context;
    private readonly IJwtTokenProvider _jwtTokenProvider;
    private readonly ISystemTime _systemTime;

    public RefreshTokenCommand(CodelyContext context, IJwtTokenProvider jwtTokenProvider, ISystemTime systemTime)
    {
        _context = context;
        _jwtTokenProvider = jwtTokenProvider;
        _systemTime = systemTime;
    }
    
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        Guard.Against
            .IsEmpty(request.RefreshToken, "Refresh token must be provided");

        var refreshTokenData = await _context.RefreshTokens
           .Where(x => x.Token == request.RefreshToken)
           .Select(x =>
               new
               {
                   RefreshToken = x,
                   x.UserId,
                   x.User.Username,
                   x.User.Email,
                   x.User.Role
               })
           .FirstOrDefaultAsync(cancellationToken);

        if (refreshTokenData is null)
        {
            throw new CodelyException("Refresh token not found");
        }

        if (refreshTokenData.RefreshToken.ValidUntil < _systemTime.Now)
        {
            throw new CodelyException("Refresh token expired");
        }

        if (refreshTokenData.RefreshToken.UsedOn.HasValue)
        {
            throw new CodelyException("Refresh token already used");
        }

        refreshTokenData.RefreshToken.UsedOn = _systemTime.Now;

        var newRefreshToken = _jwtTokenProvider.CreateRefreshToken(refreshTokenData.UserId);

        await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenProvider.Generate(refreshTokenData.UserId, refreshTokenData.Username, refreshTokenData.Email, refreshTokenData.Role);
       
        return new RefreshTokenResponse
        {
            Token = token,
            RefreshToken = newRefreshToken.Token
        };
    }
}

public sealed class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    public string? RefreshToken { get; init; }
}

public sealed class RefreshTokenResponse
{
    public string Token { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;
}