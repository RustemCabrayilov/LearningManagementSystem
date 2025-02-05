namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public interface IChatService
{
    Task<ChatResponse> GetChatUsersAsync(string userId);
    Task<ChatRequest> SendChatMessageAsync(ChatRequest request);
}