using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Exam;

public record ExamResponse(
    Guid Id,
    decimal MaxPoint,
    ExamType ExamType,
    GroupResponse Group,
    DateTime StartDate,
    DateTime EndDate,
    List<RetakeExamResponse> RetakeExams
);