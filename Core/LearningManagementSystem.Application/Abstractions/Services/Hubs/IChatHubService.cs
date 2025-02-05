using LearningManagementSystem.Application.Abstractions.Services.Chat;

namespace LearningManagementSystem.Application.Abstractions.Services.Hubs;

public interface IChatHubService
{
    Task SendMessage(ChatRequest request);
}