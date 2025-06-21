using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace IntelliBot.Core.Services;

public class OpenrouterAssistantService : IAssistantService
{
    private readonly Kernel _kernel;

    public OpenrouterAssistantService(IConfiguration configuration)
    {
        string openRouterApiKey = configuration["OPENROUTER_API_KEY"]!; 
        string openRouterModelId = configuration["OPENROUTER_MODEL_ID"]!;
        string openRouterBaseUrl = configuration["OPENROUTER_BASE_URL"]!;
        
        _kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
                modelId: openRouterModelId,
                apiKey: openRouterApiKey,
                endpoint: new Uri(openRouterBaseUrl)
            ).Build();
    }

    public async Task<string> GetChatReplyAsync(string userMessage)
    {
        string prompt = $"Пользователь сказал: {userMessage}\n\n ответь пользователю в дружественной форме";
        
        KernelFunction chatFunction =
            _kernel.CreateFunctionFromPrompt(prompt, new OpenAIPromptExecutionSettings { MaxTokens = 1000 });
        
        FunctionResult result = await _kernel.InvokeAsync(chatFunction);

        return result.GetValue<string>()?.Trim()!;
    }
}