using Refit;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentExam;

public record StudentExamRequest(
    Guid Id,
    Guid ExamId,
    Guid StudentId,
    float Point);