using DetectorPpeApi.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetectorPpeApi.Services;

/// <summary>
/// IWhatsAppService is an interface that defines the methods to send messages and media messages via WhatsApp.
/// </summary>
public interface IWhatsAppService
{
    /// <summary>
    /// SendMessageAsync is a method that sends a message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <returns>A string representing the response from the WhatsApp API.</returns>
    Task<string> SendMessageAsync(string msg, string recipientPhoneNumber);

    /// <summary>
    /// UploadMediaAsync is a method that uploads an image to the WhatsApp API.
    /// </summary>
    /// <param name="img">The image to be uploaded.</param>
    /// <returns>A string representing the media ID of the uploaded image.</returns>
    Task<string> UploadMediaAsync(IFormFile img);

    /// <summary>
    /// SendMediaMessageAsync is a method that sends a media message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="mediaId">The media ID of the image to be sent.</param>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <returns>A string representing the response from the WhatsApp API.</returns>
    Task<string> SendMediaMessageAsync(string mediaId, string recipientPhoneNumber);
}

/// <summary>
/// WhatsAppService is a service that allows sending messages and media messages via WhatsApp.
/// </summary>
public class WhatsAppService : IWhatsAppService
{
    /// <summary>
    /// HttpClient instance to send HTTP requests.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// IOptions instance to access the WhatsApp API settings.
    /// </summary>
    private readonly IOptions<WhatsAppApiSettings> _settings;

    /// <summary>
    /// string property that returns the RequestUri from the WhatsApp API settings.
    /// </summary>
    /// <returns>The RequestUri from the WhatsApp API settings.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the RequestUri is not configured.</exception>
    private string RequestUri => _settings.Value.RequestUri
        ?? throw new InvalidOperationException("RequestUri is not configured.");

    /// <summary>
    /// string property that returns the PhoneNumberId from the WhatsApp API settings.
    /// </summary>
    /// <returns>The PhoneNumberId from the WhatsApp API settings.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the PhoneNumberId is not configured.</exception>
    private string PhoneNumberId => _settings.Value.PhoneNumberId 
        ?? throw new InvalidOperationException("PhoneNumberId is not configured.");

    /// <summary>
    /// Default constructor that initializes the HttpClient and sets the Authorization header.
    /// </summary>
    /// <param name="settings">IOptions instance to access the WhatsApp API settings.</param>
    /// <param name="httpClientFactory">IHttpClientFactory instance to create an HttpClient.</param>
    public WhatsAppService(IOptions<WhatsAppApiSettings> settings, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", settings.Value.AccessToken);

        _settings = settings;
    }

    /// <summary>
    /// SendMessageAsync is a method that sends a message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <returns>A string representing the response from the WhatsApp API.</returns>
    public async Task<string> SendMessageAsync(string msg, string recipientPhoneNumber)
    {
        WhatsAppTextMessage whatsAppTextMsg = new(msg, recipientPhoneNumber);
        string httpRequestBody = JsonSerializer.Serialize(whatsAppTextMsg);

        HttpRequestMessage httpRequestMsg = new(HttpMethod.Post, $"{RequestUri}/{PhoneNumberId}/messages");
        httpRequestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        httpRequestMsg.Content = new StringContent(httpRequestBody, Encoding.UTF8, MediaTypeNames.Application.Json);

        HttpResponseMessage res = await _httpClient.SendAsync(httpRequestMsg);

        return await res.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// UploadMediaAsync is a method that uploads an image to the WhatsApp API.
    /// </summary>
    /// <param name="img">The image to be uploaded.</param>
    /// <returns>A string representing the media ID of the uploaded image.</returns>
    public async Task<string> UploadMediaAsync(IFormFile img)
    {
        string imageMediaType = img.ContentType switch
        {
            "image/jpeg" => MediaTypeNames.Image.Jpeg,
            "image/png" => MediaTypeNames.Image.Png,
            _ => throw new InvalidOperationException("Only jpg and png files are allowed.")
        };

        StreamContent fileStreamContent = new(img.OpenReadStream());
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg);

        HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, $"{RequestUri}/{PhoneNumberId}/media");
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Multipart.FormData));
        httpRequestMessage.Content = new MultipartFormDataContent
        {
            { fileStreamContent, "file", img.FileName },
            { new StringContent(imageMediaType), "type" },
            { new StringContent("whatsapp"), "messaging_product" }
        };

        HttpResponseMessage res = await _httpClient.SendAsync(httpRequestMessage);
        string resContent = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UploadMediaResponse>(resContent)?.Id ?? string.Empty;
    }

    /// <summary>
    /// SendMediaMessageAsync is a method that sends a media message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="mediaId">The media ID of the image to be sent.</param>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <returns>A string representing the response from the WhatsApp API.</returns>
    public async Task<string> SendMediaMessageAsync(string mediaId, string recipientPhoneNumber)
    {
        WhatsAppImageMessage whatsAppMediaMsg = new(mediaId, recipientPhoneNumber);
        string httpRequestBody = JsonSerializer.Serialize(whatsAppMediaMsg);

        HttpRequestMessage httpRequestMsg = new(HttpMethod.Post, $"{RequestUri}/{PhoneNumberId}/messages");
        httpRequestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        httpRequestMsg.Content = new StringContent(httpRequestBody, Encoding.UTF8, MediaTypeNames.Application.Json);

        HttpResponseMessage res = await _httpClient.SendAsync(httpRequestMsg);

        return await res.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// UploadMediaResponse is a class that represents the response from the WhatsApp API when uploading media.
    /// </summary>
    private class UploadMediaResponse
    {
        /// <summary>
        /// string property that represents the media ID of the uploaded image.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
