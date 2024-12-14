namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public record GroupRequest(
    char Code,
    string Name,
    TimeSpan StartDate,
    TimeSpan EndDate,
    DayOfWeek DayOfWeek,
    Guid TermId,
    Guid TeacherId,
    Guid SubjectId,
    Guid MajorId
);