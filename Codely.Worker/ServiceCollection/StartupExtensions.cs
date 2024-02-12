using Codely.Core.Services;
using Codely.Worker.Resolver;

namespace Codely.Worker.ServiceCollection;

public static class StartupExtensions
{
    public static void AddUserClaimsResolver(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserService, UserResolver>();
    }
}