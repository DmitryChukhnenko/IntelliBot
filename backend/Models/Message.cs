namespace IntelliBot.Core.Models;

public class Message
{
    private string _role = "user";

    private static readonly string[] sourceArray = ["system", "user", "assistant", "tool"];
    public string Role
    {
        get => _role;
        set
        {
            if (sourceArray.Contains(value.ToLower()))
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