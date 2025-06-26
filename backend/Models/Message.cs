namespace IntelliBot.Core.Models;

public class Message
{
    private string _role = "user";

    public string Role
    {
        get => _role;
        set
        {
            if (new[] { "system", "user", "assistant", "tool" }.Contains(value.ToLower()))
            {
                _role = value.ToLower();
            }
            else
            {
                _role = "user";
            }
        }
    }
    
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}