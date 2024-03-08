using DetectorPpeApi.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetectorPpeApi.Services;

public interface IWhatsAppService
{
    Task<string> SendMessageAsync(string msg, string recipientPhoneNumber);
    Task<string> UploadMediaAsync(string filePath);
    Task<string> SendMediaMessageAsync(string mediaId, string recipientPhoneNumber);
}

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<WhatsAppApiSettings> _settings;

    private string RequestUri => _settings.Value.RequestUri
        ?? throw new InvalidOperationException("RequestUri is not configured.");

    private string PhoneNumberId => _settings.Value.PhoneNumberId 
        ?? throw new InvalidOperationException("PhoneNumberId is not configured.");

    public WhatsAppService(IOptions<WhatsAppApiSettings> settings, IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", settings.Value.AccessToken);

        _settings = settings;
    }

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

    public async Task<string> UploadMediaAsync(string filePath)
    {
        StreamContent fileStreamContent = new(File.OpenRead(filePath));
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg);

        HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, $"{RequestUri}/{PhoneNumberId}/media");
        httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Multipart.FormData));
        httpRequestMessage.Content = new MultipartFormDataContent
        {
            { fileStreamContent, "file", "sample.jpg" },
            { new StringContent(MediaTypeNames.Image.Jpeg), "type" },
            { new StringContent("whatsapp"), "messaging_product" }
        };

        HttpResponseMessage res = await _httpClient.SendAsync(httpRequestMessage);
        string resContent = await res.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<UploadMediaResponse>(resContent)?.Id ?? string.Empty;
    }

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

    private class UploadMediaResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
