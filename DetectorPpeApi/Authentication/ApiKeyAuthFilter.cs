using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace DetectorPpeApi.Authentication;

/// <summary>
/// ApiKeyAuthFilter is a filter for API key authentication.
/// It checks if the API key is present in the request headers and if it is valid.
/// </summary>
/// <param name="config">IConfiguration instance to access the configuration values.</param>
public class ApiKeyAuthFilter(IConfiguration config) : IAsyncAuthorizationFilter
{
    private readonly IConfiguration _config = config;

    /// <summary>
    /// OnAuthorizationAsync is the method that is called when the filter is executed.
    /// </summary>
    /// <param name="context">AuthorizationFilterContext instance that contains the information about the request.</param>
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
