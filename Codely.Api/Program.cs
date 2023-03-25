using Codely.Api.ServiceCollection;
using Codely.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddTokenAuthConfiguration(builder.Configuration);
builder.Services.AddUserClaimsResolver(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();