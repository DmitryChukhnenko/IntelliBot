using IntelliBot.Core.Models;
using IntelliBot.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntelliBot.Core.Controllers;

[ApiController]
[Route("api/cognition")]
public class AssistantController : ControllerBase
{
    private readonly IAssistantService _assistantService;

    public AssistantController(IAssistantService assistantService)
    {
        _assistantService = assistantService;
    }

    [HttpPost]
    public async Task<ActionResult<AssistantResponse>> SendMessage([FromBody] UserQuery query)
    {
        if (string.IsNullOrWhiteSpace(query.Content))
        {
            return BadRequest("Сообщение не может быть пустым.");
        }

        string reply = await _assistantService.GetChatReplyAsync(query.Content);
        return Ok(new AssistantResponse { Reply = reply });
    }
}