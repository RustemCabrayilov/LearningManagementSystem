using LearningManagementSystem.Application.Abstractions.Services.User;

namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public interface IChatService
{
    Task<List<UserResponse>> GetChatUsersAsync(string userId);
    Task<List<ChatResponse>> GetChatMessages(ChatMessageDto request);
    Task<ChatResponse> SendChatMessageAsync(ChatRequest request);
}