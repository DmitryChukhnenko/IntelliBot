namespace IntelliBot.Core.Services;

public interface IAssistantService
{
    Task<string> GetChatReplyAsync(string userMessage);
}