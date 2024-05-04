using DetectorPpeApi.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DetectorPpeApi.Controllers;

/// <summary>
/// WeatherForecastController is a controller that helps to test if the API and the API key authentication are working.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    /// <summary>
    /// Get is a method that returns a list of weather forecasts.
    /// Used to test if the API and the API key authentication are working.
    /// </summary>
    [HttpGet]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
