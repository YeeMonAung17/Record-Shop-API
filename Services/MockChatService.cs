using Record_Shop.AI.Models;

namespace Record_Shop.AI.Services;

public class MockChatService : IChatService
{
    public Task<ChatResponse> GetReplyAsync(
        string userMessage,
        List<ChatMessage>? history,
        CancellationToken cancellationToken = default)
    {
        var reply = $"Mock AI response: I received your message: '{userMessage}'";

        return Task.FromResult(new ChatResponse
        {
            Success = true,
            Reply = reply,
            Error = null
        });
    }
}