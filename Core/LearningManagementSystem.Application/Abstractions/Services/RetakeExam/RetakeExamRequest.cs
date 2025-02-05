using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.RetakeExam;

public record RetakeExamRequest(
    Guid ExamId,
    DateTime Deadline,
    RetakeExamType RetakeExamType,
    decimal Price,
    List<IFormFile> Files);