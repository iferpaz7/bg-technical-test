namespace BG.API.Middleware;

public class ClientIdValidationMiddleware
{
    private readonly RequestDelegate _next;

    public ClientIdValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Only validate specific paths or methods if necessary
        if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
        {
            if (!context.Request.Headers.TryGetValue("X-Client-Id", out var clientId) || clientId != "app-bg-tech-test")
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new { message = "Invalid or missing X-Client-Id header" });
                return;
            }
        }

        await _next(context); // Continue to the next middleware or endpoint
    }
}
