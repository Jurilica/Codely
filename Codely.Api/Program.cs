using System.Text.Json.Serialization;
using Codely.Api.Constants;
using Codely.Api.Middleware;
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/swagger/{SwaggerConstants.User}/swagger.json", "User");
    c.SwaggerEndpoint($"/swagger/{SwaggerConstants.Admin}/swagger.json", "Admin");
    c.SwaggerEndpoint($"/swagger/{SwaggerConstants.Shared}/swagger.json", "Shared");
    c.DisplayRequestDuration();
});

app.UseHttpsRedirection();

app.UseMiddleware<ResponseRewriteMiddleware>();

app.UseCors(x => 
    x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.MapHangfireDashboard(new DashboardOptions());

app.Run();