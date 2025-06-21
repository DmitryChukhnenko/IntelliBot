using IntelliBot.WebApp.Models;
using System.Net.Http.Json;

namespace IntelliBot.WebApp.Services;

public class CoreService : ICoreService
{
    private readonly HttpClient _httpClient;
    private readonly string _backendUrl;

    public CoreService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _backendUrl = config["API_ENDPOINT"]!;
        
        if (string.IsNullOrWhiteSpace(_backendUrl))
        {
            throw new ArgumentException("API_ENDPOINT is not configured");
        }
        
        if (!Uri.IsWellFormedUriString(_backendUrl, UriKind.Absolute))
        {
            throw new ArgumentException($"Invalid API_ENDPOINT: {_backendUrl}");
        }
    }

    public async Task<AssistantResponse?> ProcessInputAsync(string message)
    {
        try
        {
            UserQuery query = new() { Content = message };
            var response = await _httpClient.PostAsJsonAsync(_backendUrl, query);
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AssistantResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");
            return new AssistantResponse { Reply = $"Ошибка: {ex.Message}" };
        }
    }
}