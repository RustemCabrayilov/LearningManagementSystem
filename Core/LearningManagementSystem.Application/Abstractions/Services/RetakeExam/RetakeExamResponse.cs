using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.RetakeExam;

public record RetakeExamResponse(
    Guid Id,
    ExamResponse Exam,
    DateTime Deadline,
    RetakeExamType RetakeExamType,
    decimal Price);