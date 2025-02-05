namespace LearningManagementSystem.Application.Abstractions.Services.Transcript;

public record TranscriptRequest(
    Guid StudentId,
    Guid GroupId,
    float TotalPoint
    );