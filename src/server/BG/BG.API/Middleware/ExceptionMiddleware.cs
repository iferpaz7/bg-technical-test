using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Diagnostics;

namespace BG.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestBody = string.Empty;
        var startTime = Stopwatch.GetTimestamp();

        try
        {
            if (IsRequestWithBody(context.Request.Method))
            {
                requestBody = await ReadRequestBodyAsync(context);
            }
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, requestBody);
        }
        finally
        {
            var duration = Stopwatch.GetElapsedTime(startTime);
            _logger.LogInformation("Request {Method} {Path} completed in {Duration}ms",
                context.Request.Method, context.Request.Path, duration.TotalMilliseconds);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, string requestBody)
    {
        var queryString = context.Request.QueryString.ToString();
        _logger.LogError(ex,
            "Exception: {Message}. Request: {Method} {Path}. Body: {Body}. Query: {Query}",
            ex.Message, context.Request.Method, context.Request.Path, requestBody, queryString);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "Server Error" + (ex is NullReferenceException ? " (Null Reference)" : ""),
            Detail = _env.IsDevelopment() ? ex.StackTrace : null,
            Type = ex.GetType().Name,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static bool IsRequestWithBody(string method) =>
        method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Patch;

    private static async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
        var content = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        return content;
    }
}