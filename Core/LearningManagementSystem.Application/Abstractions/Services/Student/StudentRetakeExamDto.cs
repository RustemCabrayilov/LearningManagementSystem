namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentRetakeExamDto(
    Guid StudentId,
    Guid RetakeExamId
    );