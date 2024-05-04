namespace DetectorPpeApi.Models;

/// <summary>
/// WhatsAppApiSettings is a class that contains the settings required to access the WhatsApp API.
/// </summary>
public class WhatsAppApiSettings
{
    /// <summary>
    /// string property that returns the AccessToken from the WhatsApp API settings.
    /// </summary>
    public string? AccessToken { get; init; }

    /// <summary>
    /// string property that returns the ApiUrl from the WhatsApp API settings.
    /// </summary>
    public string? ApiUrl { get; init; }

    /// <summary>
    /// string property that returns the ApiVersion from the WhatsApp API settings.
    /// </summary>
    public string? ApiVersion { get; init; }

    /// <summary>
    /// string property that returns the PhoneNumberId from the WhatsApp API settings.
    /// </summary>
    public string? PhoneNumberId { get; init; }

    /// <summary>
    /// string property that returns the RequestUri from the WhatsApp API settings.
    /// </summary>
    public string? RequestUri => $"{ApiUrl}/v{ApiVersion}";
}
