using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Codely.Core.Types;

namespace Codely.Api.Middleware;

public class ResponseRewriteMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseRewriteMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (CodelyException codelyException)
        {
            await WriteResponse(context, codelyException.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception)
        {
            await WriteResponse(context, "Unhandled exception occured", HttpStatusCode.InternalServerError);
        }
    }
    
    private async Task WriteResponse(HttpContext context, string message, HttpStatusCode statusCode)
    {
        var details = new ErrorData
        {
            Message = message
        };

        var result = JsonSerializer.Serialize(details);
        
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        
        await context.Response.WriteAsync(result);
    }
}

public class ErrorData
{
    public string Message { get; set; } = string.Empty;
}