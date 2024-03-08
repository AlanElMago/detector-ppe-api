using System.Text.Json.Serialization;

namespace DetectorPpeApi.Models;

public class WhatsAppImageMessage(string mediaId, string recipientPhoneNumber)
{
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; } = "whatsapp";

    [JsonPropertyName("recipient_type")]
    public string RecipientType { get; set; } = "individual";

    [JsonPropertyName("to")]
    public string? To { get; set; } = recipientPhoneNumber;

    [JsonPropertyName("type")]
    public string? Type { get; set; } = "image";

    [JsonPropertyName("image")]
    public WhatsAppImage? Image { get; set; } = new(mediaId);

    public class WhatsAppImage(string mediaId)
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; } = mediaId;
    }
}
