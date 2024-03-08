namespace DetectorPpeApi.Models;

public class WhatsAppApiSettings
{
    public string? AccessToken { get; init; }

    public string? ApiUrl { get; init; }

    public string? ApiVersion { get; init; }

    public string? PhoneNumberId { get; init; }

    public string? RequestUri => $"{ApiUrl}/v{ApiVersion}";
}
