namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public record GroupRequest(
    char Code,
    string Name,
    int Credit,
    Guid TeacherId,
    bool CanApply,
    Guid SubjectId,
    Guid MajorId
);