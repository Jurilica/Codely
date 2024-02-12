using Codely.Core.Configuration;
using Codely.Worker.ServiceCollection;
using Hangfire;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHangfireServer();
        services.AddCoreServices(context.Configuration);
        services.AddHangfire(context.Configuration);
        services.AddUserClaimsResolver();
    })
    .Build();

host.Run();