using System.Text.Json.Serialization;

namespace DetectorPpeApi.Models;

/// <summary>
/// WhatsAppImageMessage is a class that represents a media message to be sent via WhatsApp.
/// </summary>
/// <param name="mediaId">The media ID of the image to be sent.</param>
/// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
public class WhatsAppImageMessage(string mediaId, string recipientPhoneNumber)
{
    /// <summary>
    /// string property that returns the MessagingProduct from the WhatsApp API settings.
    /// By default, it is set to "whatsapp", which represents that the message is sent via WhatsApp.
    /// </summary>
    [JsonPropertyName("messaging_product")]
    public string MessagingProduct { get; set; } = "whatsapp";

    /// <summary>
    /// string property that returns the RecipientType from the WhatsApp API settings.
    /// By default, it is set to "individual", which represents that the message is sent to an individual recipient.
    /// </summary>
    [JsonPropertyName("recipient_type")]
    public string RecipientType { get; set; } = "individual";

    /// <summary>
    /// string property that returns the recipient phone number.
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; set; } = recipientPhoneNumber;

    /// <summary>
    /// string property that returns the type of the message.
    /// By default, it is set to "image", which represents that the message is an image.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "image";

    /// <summary>
    /// WhatsAppImage property that returns the image to be sent.
    /// </summary>
    [JsonPropertyName("image")]
    public WhatsAppImage? Image { get; set; } = new(mediaId);

    /// <summary>
    /// WhatsAppImage is a class that represents an image to be sent via WhatsApp.
    /// </summary>
    /// <param name="mediaId">The media ID of the image to be sent.</param>
    public class WhatsAppImage(string mediaId)
    {
        /// <summary>
        /// string property that returns the media ID of the image to be sent.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; } = mediaId;
    }
}
