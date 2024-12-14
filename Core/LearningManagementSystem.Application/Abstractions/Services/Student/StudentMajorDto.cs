namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentMajorDto(
    Guid StudentId,
    Guid MajorId);