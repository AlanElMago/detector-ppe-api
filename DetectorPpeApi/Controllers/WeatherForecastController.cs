using DetectorPpeApi.Authentication;
using DetectorPpeApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace DetectorPpeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(
    IWhatsAppService whatsAppService,
    ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger = logger;
    private readonly IWhatsAppService _whatsAppService = whatsAppService;

    [HttpGet]
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

    [HttpPost("SendToWhatsApp/{recipientPhoneNumber}")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SendWeatherForcastToWhatsApp([FromRoute] string recipientPhoneNumber)
    {
        if (string.IsNullOrEmpty(recipientPhoneNumber))
        {
            return BadRequest("Recipient phone number is required.");
        }

        string date = DateOnly.FromDateTime(DateTime.Now).ToString("d");
        string temperature = Random.Shared.Next(-20, 55).ToString();
        string summary = Summaries[Random.Shared.Next(Summaries.Length)];

        string msg = $"Weather forecast for {date}: {temperature}°C and {summary}";
        string res = await _whatsAppService.SendMessageAsync(msg, recipientPhoneNumber);

        return Ok(res);
    }
}
