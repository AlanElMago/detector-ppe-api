using System.Text.Json.Serialization;

namespace DetectorPpeApi.Models;

/// <summary>
/// WhatsAppTextMessage is a class that represents a message to be sent via WhatsApp.
/// </summary>
/// <param name="msg">The message to be sent.</param>
/// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
public class WhatsAppTextMessage(string msg, string recipientPhoneNumber)
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
    /// By default, it is set to "text", which represents that the message is a text message.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "text";

    /// <summary>
    /// WhatsAppText property that returns the text message to be sent.
    /// </summary>
    [JsonPropertyName("text")]
    public WhatsAppText? Text { get; set; } = new(msg);

    /// <summary>
    /// WhatsAppText is a class that represents a text message to be sent via WhatsApp.
    /// </summary>
    public class WhatsAppText(string msg)
    {
        /// <summary>
        /// bool property that returns true if the preview URL option is enabled; otherwise, false.
        /// By default, it is set to false, which represents that the preview URL option is disabled.
        /// </summary>
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; set; } = false;

        /// <summary>
        /// string property that returns the body of the text message.
        /// </summary>
        [JsonPropertyName("body")]
        public string? Body { get; set; } = msg;
    }
}
