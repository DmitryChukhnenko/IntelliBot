namespace IntelliBot.Core.Services;

public interface IAssistantService
{
    Task<string> ProcessConversation(string sessionId, string userMessag);
    void CleanupExpiredConversations();
}