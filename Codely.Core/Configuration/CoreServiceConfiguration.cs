using Codely.Core.Configuration.Settings;
using Codely.Core.Data;
using Codely.Core.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddTransient<ICodeExecutionService, CodeExecutionService>();
        services.AddTransient<IJwtTokenProvider, JwtTokenProvider>();
        services.AddTransient<ITestCaseService, TestCaseService>();
        services.AddTransient<ITestCaseJobs, TestCaseJobs>();

        services.AddSingleton<ISystemTime, SystemTime>();
    }

    public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(hangfireConfiguration => hangfireConfiguration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseFilter(
                new AutomaticRetryAttribute
                {
                    Attempts = 0
                })
            .UsePostgreSqlStorage(
                configuration.GetConnectionString("HangfireContext"),
                new PostgreSqlStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromSeconds(1)
                }));
    }
}