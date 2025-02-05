namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public record ChatRequest(
   string UserId,
string ToUserId,
string Message 
    );