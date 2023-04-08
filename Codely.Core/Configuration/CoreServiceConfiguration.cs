using Codely.Core.Configuration.Settings;
using Codely.Core.Data;
using Codely.Core.Gateways;
using Codely.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Codely.Core.Configuration;

public static class CoreServiceConfiguration
{
    public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CoreServiceConfiguration).Assembly));
        services.AddDbContext<CodelyContext>(x => x
            .UseNpgsql(configuration.GetConnectionString(nameof(CodelyContext)))
            .UseSnakeCaseNamingConvention());
        
        var jwtSettingsSection = configuration.GetSection(nameof(JwtSettings));
        services.Configure<JwtSettings>(jwtSettingsSection);

        services.AddRefitClient<ICodeTranslationClient>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri("https://emkc.org"));
        
        services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
        
        services.AddSingleton<ISystemTime,SystemTime>();
    }
}