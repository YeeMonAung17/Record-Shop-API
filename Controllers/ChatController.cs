using Microsoft.AspNetCore.Mvc;
using Record_Shop.AI.Models;
using Record_Shop.AI.Services;

namespace Record_Shop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(
        [FromBody] ChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new
            {
                error = "Message is required."
            });
        }

        var response = await _chatService.GetReplyAsync(
            request.Message,
            request.History,
            cancellationToken
        );

        if (!response.Success)
        {
            return StatusCode(500, response);
        }

        return Ok(response);
    }
}