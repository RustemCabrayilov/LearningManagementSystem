namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentLessonDto(
    Guid StudentId,
    Guid LessonId);