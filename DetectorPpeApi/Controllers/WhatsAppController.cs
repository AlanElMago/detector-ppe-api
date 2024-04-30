using DetectorPpeApi.Authentication;
using DetectorPpeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DetectorPpeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsAppController(IWhatsAppService whatsAppService) : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService = whatsAppService;

    [HttpPost("SendMessage/{recipientPhoneNumber}")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SendMessage(string recipientPhoneNumber, [FromBody] string msg)
    {
        if (string.IsNullOrEmpty(recipientPhoneNumber))
        {
            return BadRequest("Recipient phone number is required.");
        }

        if (string.IsNullOrEmpty(msg))
        {
            return BadRequest("Message is required.");
        }

        string res = await _whatsAppService.SendMessageAsync(msg, recipientPhoneNumber);

        return Ok(res);
    }

    [HttpPost("SendMediaMessage/{recipientPhoneNumber}")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SendMediaMessage(string recipientPhoneNumber, [FromForm] IFormFile img)
    {
        if (string.IsNullOrEmpty(recipientPhoneNumber))
        {
            return BadRequest("Recipient phone number is required.");
        }

        if (img == null)
        {
            return BadRequest("Image is required.");
        }

        if (img.ContentType != "image/jpeg" && img.ContentType != "image/png")
        {
            return BadRequest("Only jpg and png files are allowed.");
        }

        string mediaId = await _whatsAppService.UploadMediaAsync(img);

        if (mediaId == string.Empty)
        {
            return BadRequest("Error uploading media.");
        }

        string res = await _whatsAppService.SendMediaMessageAsync(mediaId, recipientPhoneNumber);

        return Ok(res);
    }
}
