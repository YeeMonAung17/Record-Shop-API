using Azure.AI.OpenAI;
using OpenAI.Chat;
using Record_Shop.AI.Models;
using System.ClientModel;

namespace Record_Shop.AI.Services;

public class ChatService : IChatService
{
    private readonly ChatClient _chatClient;

    public ChatService(IConfiguration configuration)
    {
        var endpoint = new Uri(configuration["AzureAi:Endpoint"]!);
        var apiKey = configuration["AzureAi:ApiKey"]!;
        var deploymentName = configuration["AzureAi:DeploymentName"]!;

        var azureClient = new AzureOpenAIClient(
            endpoint,
            new ApiKeyCredential(apiKey)
        );

        _chatClient = azureClient.GetChatClient(deploymentName);
    }

    public async Task<ChatResponse> GetReplyAsync(
        string userMessage,
        List<Record_Shop.AI.Models.ChatMessage>? history,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var messages = new List<OpenAI.Chat.ChatMessage>
            {
                new SystemChatMessage(
                    "You are a helpful assistant for Vinyl Vault, a record shop inventory app. Keep answers clear and concise."
                )
            };

            if (history != null)
            {
                foreach (var message in history)
                {
                    if (message.Role.Equals("user", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new UserChatMessage(message.Content));
                    }
                    else if (message.Role.Equals("assistant", StringComparison.OrdinalIgnoreCase))
                    {
                        messages.Add(new AssistantChatMessage(message.Content));
                    }
                }
            }

            messages.Add(new UserChatMessage(userMessage));

            var result = await _chatClient.CompleteChatAsync(
                messages,
                cancellationToken: cancellationToken
            );

            var reply = result.Value.Content.Count > 0
                ? result.Value.Content[0].Text
                : "I couldn't generate a response. Please try again.";

            return new ChatResponse
            {
                Success = true,
                Reply = reply,
                Error = null
            };
        }
        catch (Exception ex)
        {
            return new ChatResponse
            {
                Success = false,
                Reply = string.Empty,
                Error = ex.Message
            };
        }
    }
}