using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using IntelliBot.Core.Models;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Collections.Concurrent;
using IntelliBot.Core.Config;

namespace IntelliBot.Core.Services;

public class OpenrouterAssistantService : IAssistantService
{
    private readonly Kernel _kernel;
    private readonly MemoryConfig _memoryConfig;
    private readonly ConcurrentDictionary<string, Conversation> _conversations = new();

    public OpenrouterAssistantService(IConfiguration config)
    {
        var expiration = config.GetValue<int>("MEMORY_EXPIRATION_MINUTES", 30);
        var maxHistory = config.GetValue<int>("MAX_CONVERSATION_HISTORY", 10);
        _memoryConfig = new MemoryConfig(expiration, maxHistory);

        string apiKey = config["OPENROUTER_API_KEY"]
            ?? throw new ArgumentException("OPENROUTER_API_KEY is not configured");

        string modelId = config["OPENROUTER_MODEL_ID"]
            ?? "mistralai/mistral-7b-instruct:free";

        string baseUrl = config["OPENROUTER_BASE_URL"]
            ?? "https://openrouter.ai/api/v1";

        _kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: modelId,
                apiKey: apiKey,
                endpoint: new Uri(baseUrl))
            .Build();
    }

    public async Task<string> ProcessConversation(string sessionId, string userMessage)
    {
        CleanupExpiredConversations();

        var conversation = _conversations.GetOrAdd(sessionId, id => new Conversation
        {
            SessionId = id,
            LastAccess = DateTime.UtcNow
        });

        conversation.LastAccess = DateTime.UtcNow;

        conversation.Messages.Add(new Message
        {
            Role = "user",
            Content = userMessage
        });

        if (conversation.Messages.Count > _memoryConfig.MaxHistory)
        {
            conversation.Messages.RemoveRange(0, conversation.Messages.Count - _memoryConfig.MaxHistory);
        }

        var chatHistory = new ChatHistory();

        foreach (var msg in conversation.Messages)
        {
            AuthorRole role = ConvertToAuthorRole(msg.Role);
            chatHistory.AddMessage(role, msg.Content);
        }

        var chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
        var reply = await chatCompletion.GetChatMessageContentAsync(
            chatHistory,
            executionSettings: new OpenAIPromptExecutionSettings
            {
                MaxTokens = 1000,
                Temperature = 0.7
            },
            kernel: _kernel);

        reply.Content ??= "Empty response";

        conversation.Messages.Add(new Message
        {
            Role = "assistant",
            Content = reply.Content
        });

        return reply.Content;
    }

    public void CleanupExpiredConversations()
    {
        var expirationTime = DateTime.UtcNow.AddMinutes(-_memoryConfig.ExpirationMinutes);
        var expiredSessions = _conversations
            .Where(kvp => kvp.Value.LastAccess < expirationTime)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var sessionId in expiredSessions)
        {
            _conversations.TryRemove(sessionId, out _);
        }
    }
    
    private AuthorRole ConvertToAuthorRole(string role)
    {
        return role.ToLower() switch
        {
            "system" => AuthorRole.System,
            "user" => AuthorRole.User,
            "assistant" => AuthorRole.Assistant,
            "tool" => AuthorRole.Tool,
            _ => AuthorRole.User 
        };
    }
}