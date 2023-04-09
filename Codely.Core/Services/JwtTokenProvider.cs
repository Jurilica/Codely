using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Codely.Core.Configuration.Settings;
using Codely.Core.Data.Entities;
using Codely.Core.Types.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Codely.Core.Services;

public interface IJwtTokenProvider
{
    string Generate(int userId, string username, string email, Role role);

    RefreshToken CreateRefreshToken(int userId);
}

public sealed class JwtTokenProvider : IJwtTokenProvider
{
    private readonly JwtSettings _jwtSettings;
    private readonly ISystemTime _systemTime;
    
    public JwtTokenProvider(IOptions<JwtSettings> options, ISystemTime systemTime)
    {
        _systemTime = systemTime;
        _jwtSettings = options.Value;
    }
    
    public string Generate(int userId, string username, string email, Role role)
    {
        var claims = new List<Claim>
        {
            new(UserClaimType.UserId, userId.ToString()),
            new(UserClaimType.Name, username),
            new(UserClaimType.Email, email),
            new(UserClaimType.Role, role.ToString())
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires:_systemTime.Now.AddHours(_jwtSettings.TokenLifetimeMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken CreateRefreshToken(int userId)
    {
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = userId,
            ValidUntil = _systemTime.Now.AddDays(_jwtSettings.RefreshTokenLifetimeDays)
        };
    }
}