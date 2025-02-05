using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Exam;

public record ExamRequest(
    decimal MaxPoint,
    ExamType ExamType,
    Guid GroupId,
    DateTime StartDate,
    DateTime EndDate
    );