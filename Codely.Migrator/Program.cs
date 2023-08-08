using Codely.Core.Configuration;
using Codely.Core.Data;
using Codely.Core.Data.Entities;
using Codely.Core.Types.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddCoreServices(context.Configuration);
    })
    .Build();

var context = host.Services.GetRequiredService<CodelyContext>();

await context.Database.EnsureDeletedAsync();
await context.Database.EnsureCreatedAsync();

var testCases = new List<TestCase>
{
    new()
    {
        Input = "1 1",
        Output = "2"
    },
    new()
    {
        Input = "3 3",
        Output = "6"
    }
};

var problem = new Problem
{
  Title  = "Addition",
  Description = "Add 2 numbers",
  TestCases = testCases
};

context.Problems.Add(problem);

await context.SaveChangesAsync();