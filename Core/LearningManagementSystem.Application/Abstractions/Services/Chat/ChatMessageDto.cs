namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public record ChatMessageDto
(
    string UserId,
    string ToUserId
);