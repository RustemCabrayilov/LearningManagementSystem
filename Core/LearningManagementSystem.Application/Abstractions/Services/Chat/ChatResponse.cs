using LearningManagementSystem.Application.Abstractions.Services.User;

namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public record ChatResponse(
    Guid Id,
    string UserId,
    string ToUserId,
    string Message,
    DateTime Date
);