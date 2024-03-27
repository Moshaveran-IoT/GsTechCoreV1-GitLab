using System.Diagnostics;

namespace Moshaveran.GsTech.Mqtt.API.Middlewares;

public class LoggerMiddleware
{
    private readonly Func<HttpContext, Task> _activeInvoker;
    private readonly ILogger<LoggerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        (this._next, this._logger) = (next, logger);

        this._activeInvoker = this._logger.IsEnabled(LogLevel.Debug) || this._logger.IsEnabled(LogLevel.Trace)
            ? this.InvokeFull
            : this.InvokeSimple;
    }

    public Task Invoke(HttpContext httpContext)
        => this._activeInvoker(httpContext);

    private async Task InvokeFull(HttpContext httpContext)
    {
        if (httpContext.Request.Method != "OPTIONS")
        {
            this._logger.LogDebug(new EventId(this.GetHashCode(), nameof(LoggerMiddleware)), "Calling {API}", httpContext.Request.Path);
        }

        var stopwatch = Stopwatch.StartNew();
        await this._next(httpContext);
        stopwatch.Stop();

        if (httpContext.Request.Method != "OPTIONS")
        {
            this._logger.LogTrace(new EventId(this.GetHashCode(), nameof(LoggerMiddleware)), "Called  {API} with status code: {StatusCode} in {Elapsed}", httpContext.Request.Path, httpContext.Response.StatusCode, stopwatch.Elapsed);
        }
    }

    private async Task InvokeSimple(HttpContext httpContext)
        => await this._next(httpContext);
}

public static class LoggerMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<LoggerMiddleware>();
}