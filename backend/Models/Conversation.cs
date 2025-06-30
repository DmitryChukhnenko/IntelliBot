namespace IntelliBot.Core.Models;

public class Conversation
{
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public List<Message> Messages { get; set; } = [];
    public DateTime LastAccess { get; internal set; }
}