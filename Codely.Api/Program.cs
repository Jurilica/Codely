using System.Text.Json.Serialization;
using Codely.Api.ServiceCollection;
using Codely.Core.Configuration;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddTokenAuthConfiguration(builder.Configuration);
builder.Services.AddUserClaimsResolver(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddHangfire(builder.Configuration);
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.MapHangfireDashboard(new DashboardOptions());

app.Run();