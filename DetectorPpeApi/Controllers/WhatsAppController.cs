using DetectorPpeApi.Authentication;
using DetectorPpeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DetectorPpeApi.Controllers;

/// <summary>
/// WhatsAppController is a controller that allows sending messages and media messages via WhatsApp.
/// </summary>
/// <param name="whatsAppService">IWhatsAppService instance to access the WhatsApp service methods.</param>
[ApiController]
[Route("api/[controller]")]
public class WhatsAppController(IWhatsAppService whatsAppService) : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService = whatsAppService;

    /// <summary>
    /// SendMessage is a method that sends a message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <param name="msg">The message to be sent.</param>
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

    /// <summary>
    /// SendMediaMessage is a method that sends a media message to a recipient via WhatsApp.
    /// </summary>
    /// <param name="recipientPhoneNumber">The phone number of the recipient.</param>
    /// <param name="img">The image to be sent.</param>
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
