using Codely.Core.Configuration.Settings;
using Codely.Core.Data;
using Codely.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Codely.Core.Configuration;

public static class CoreServiceConfiguration
{
    public static void AddCoreServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CoreServiceConfiguration).Assembly));
        service.AddDbContext<CodelyContext>(x => x
            .UseNpgsql(configuration.GetConnectionString(nameof(CodelyContext)))
            .UseSnakeCaseNamingConvention());

        service.AddTransient<IJwtTokenProvider, JwtTokenProvider>();

        var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));
        service.Configure<JwtSettings>(jwtSettingsSection);
    }
}