using System.Net;
using System.Text.Json;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Moshaveran.GsTech.Mqtt.API.Middlewares;

public sealed class GlobalExceptionHander(ILogger<GlobalExceptionHander> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);

        var details = new ProblemDetails
        {
            Detail = $"API Error {exception.Message}",
            Instance = "IoT API",
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "API Error",
            Type = "Server Error"
        };
        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }
}