using System.Text.Json;
using BackendTestTask.Data;
using BackendTestTask.Models;
using BackendTestTask.Exceptions;

namespace BackendTestTask.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        try
        {
            await _next(context);
        }
        catch (SecureException ex)
        {
            await HandleExceptionAsync(context, ex, "Secure", dbContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, "Exception", dbContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string exceptionType, AppDbContext dbContext)
    {
        var eventId = Guid.NewGuid();
        var exceptionLog = new ExceptionLog
        {
            EventId = eventId,
            QueryParameters = context.Request.QueryString.ToString(),
            BodyParameters = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form) : "",
            StackTrace = exception.StackTrace
        };

        dbContext.ExceptionLogs.Add(exceptionLog);
        await dbContext.SaveChangesAsync();

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            type = exceptionType,
            id = eventId.ToString(),
            data = new { message = exceptionType == "Secure" ? exception.Message : $"Internal server error ID = {eventId}" }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

