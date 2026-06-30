using Record_Shop.AI.Models;

namespace Record_Shop.AI.Services;

public interface IChatService
{
    Task<ChatResponse> GetReplyAsync(
        string userMessage,
        List<ChatMessage>? history,
        CancellationToken cancellationToken = default);
}
