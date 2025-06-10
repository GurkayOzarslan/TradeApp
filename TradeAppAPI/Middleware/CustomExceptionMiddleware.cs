using System.Net;
using System.Text.Json;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);

            if (httpContext.Response.StatusCode >= 400 && httpContext.Response.StatusCode < 600 && !httpContext.Response.HasStarted)
            {
                await WriteProblemDetailsAsync(httpContext, null, httpContext.Response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await WriteProblemDetailsAsync(httpContext, ex, (int)HttpStatusCode.InternalServerError);
        }
    }

    private static async Task WriteProblemDetailsAsync(HttpContext context, Exception? exception, int statusCode)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        string title = statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Unexpected Error"
        };

        string type = $"https://httpstatuses.com/{statusCode}";

        var problemDetails = new
        {
            type,
            title,
            status = statusCode,
            detail = exception?.Message ?? title,
            ErrorDetail = exception == null ? null : new[]
            {
                new
                {
                    message = exception.Message,
                    type = exception.GetType().ToString(),
                    raw = exception.ToString()
                }
            },
            traceId = context.TraceIdentifier
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var result = JsonSerializer.Serialize(problemDetails, options);

        await context.Response.WriteAsync(result);
    }
}
