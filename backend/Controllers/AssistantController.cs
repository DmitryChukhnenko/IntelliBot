using IntelliBot.Core.Models;
using IntelliBot.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IntelliBot.Core.Controllers;

[ApiController]
[Route("api/conversation")]
public class AssistantController : ControllerBase
{
    private readonly IAssistantService _assistantService;
    private readonly ILogger<AssistantController> _logger;

    public AssistantController(
        IAssistantService assistantService,
        ILogger<AssistantController> logger)
    {
        _assistantService = assistantService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<AssistantResponse>> ProcessMessage([FromBody] UserQuery query)
    {
        try
        {
            _logger.LogInformation("Processing message for session: {SessionId}", query.SessionId);
            _logger.LogDebug("Message content: {Content}", query.Content);
            
            string reply = await _assistantService.ProcessConversation(
                query.SessionId, 
                query.Content);
            
            return Ok(new AssistantResponse { Reply = reply });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message for session: {SessionId}. Error: {ErrorMessage}", 
                query.SessionId, ex.Message);
                
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}