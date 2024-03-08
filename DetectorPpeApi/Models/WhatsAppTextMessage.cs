using System.Text.Json.Serialization;

namespace DetectorPpeApi.Models;

public class WhatsAppTextMessage(string msg, string recipientPhoneNumber)
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; } = "whatsapp";

    [JsonPropertyName("recipient_type")]
    public string RecipientType { get; set; } = "individual";

    [JsonPropertyName("to")]
    public string? To { get; set; } = recipientPhoneNumber;

    [JsonPropertyName("type")]
    public string? Type { get; set; } = "text";

    [JsonPropertyName("text")]
    public WhatsAppText? Text { get; set; } = new(msg);

    public class WhatsAppText(string msg)
    {
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; } = false;

        [JsonPropertyName("body")]
        public string? Body { get; set; } = msg;
    }
}
