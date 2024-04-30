using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace DetectorPpeApi.Authentication;

public class ApiKeyAuthFilter(IConfiguration config) : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _config = config;

    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(
            AuthConstants.ApiKeyHeaderName,
            out StringValues extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key is missing");

            return Task.CompletedTask;
        }

        string? apiKey = _config.GetValue<string>(AuthConstants.ApiKeySectionName);

        if (apiKey != extractedApiKey)
        {
            context.Result = new UnauthorizedObjectResult("API Key is invalid");
        }

        return Task.CompletedTask;
    }
}
