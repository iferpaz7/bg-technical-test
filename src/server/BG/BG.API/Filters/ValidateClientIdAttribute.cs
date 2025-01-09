using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BG.API.Filters;

public class ValidateClientIdAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _expectedClientId;

    public ValidateClientIdAttribute(string expectedClientId)
    {
        _expectedClientId = expectedClientId;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;

        if (!request.Headers.TryGetValue("X-Client-Id", out var clientId) || clientId != _expectedClientId)
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Invalid or missing X-Client-Id header" });
            return;
        }

        await next(); // Proceed to the action if the header is valid
    }
}
