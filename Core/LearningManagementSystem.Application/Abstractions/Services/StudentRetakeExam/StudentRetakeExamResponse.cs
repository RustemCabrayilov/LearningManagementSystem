using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;

public record StudentRetakeExamResponse(
    Guid Id,
    StudentResponse Student,
    RetakeExamResponse RetakeExam,
    TermResponse Term,
    Status Status,
    float NewPoint
    );