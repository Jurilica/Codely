namespace Codely.Core.Configuration.Settings;

public class JwtSettings
{
    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;
    
    public string SecretKey { get; init; } = string.Empty;
    
    public int TokenLifetimeMinutes { get; init; }
    
    public int RefreshTokenLifetimeDays { get; init; }
}