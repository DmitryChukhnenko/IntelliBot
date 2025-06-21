using IntelliBot.WebApp.Models;

namespace IntelliBot.WebApp.Services;

public interface ICoreService
{ 
    Task<AssistantResponse?> ProcessInputAsync(string message);
}