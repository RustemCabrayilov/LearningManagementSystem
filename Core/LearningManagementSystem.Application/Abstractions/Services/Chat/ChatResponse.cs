using LearningManagementSystem.Application.Abstractions.Services.User;

namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public record ChatResponse(
    List<UserResponse> Users = null);