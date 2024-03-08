using DetectorPpeApi.Authentication;
using DetectorPpeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DetectorPpeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemesController(IWhatsAppService whatsAppService) : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService = whatsAppService;

    private readonly string[] _picturePaths =
    [
        "Assets/Memes/noice.jpg",
        "Assets/Memes/smiling-old-man.jpg",
        "Assets/Memes/the-rock-raised-eyebrow.jpg"
    ];

    [HttpPost("SendToWhatsApp/{recipientPhoneNumber}")]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public async Task<IActionResult> SendToWhatsApp(string recipientPhoneNumber)
    {
        if (string.IsNullOrEmpty(recipientPhoneNumber))
        {
            return BadRequest("Recipient phone number is required.");
        }

        string picturePath = _picturePaths[Random.Shared.Next(_picturePaths.Length)];
        string mediaId = await _whatsAppService.UploadMediaAsync(picturePath);

        if (mediaId == string.Empty)
        {
            return BadRequest("Error uploading media.");
        }

        string res = await _whatsAppService.SendMediaMessageAsync(mediaId, recipientPhoneNumber);

        return Ok(res);
    }
}
