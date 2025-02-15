using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public record MajorResponse(
    Guid Id,
    string Title,
    float Point,
    string EducationLanguage,
    decimal TuitionFee,
    bool StateFunded,
    FacultyResponse Faculty);