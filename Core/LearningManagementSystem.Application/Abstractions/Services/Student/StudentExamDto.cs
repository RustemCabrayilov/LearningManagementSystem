namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentExamDto(
    Guid StudentId,
    Guid ExamId,
    float Point);