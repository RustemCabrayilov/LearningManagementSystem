namespace LearningManagementSystem.Application.Abstractions.Services.GeminiAI;

public interface IGeminiAIService
{
    ValueTask<string> GenerateTextAsync(string prompt);
}