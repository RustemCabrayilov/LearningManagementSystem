using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentRetakeExamDto(
    Guid StudentId,
    Guid RetakeExamId,
    Status Status,
    float NewPoint
    );