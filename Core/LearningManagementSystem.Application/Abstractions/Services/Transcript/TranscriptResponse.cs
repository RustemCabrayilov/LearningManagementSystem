using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Transcript;

public record TranscriptResponse(
    Guid Id,
    StudentResponse Student,
    GroupResponse Group,
    float TotalPoint
    );