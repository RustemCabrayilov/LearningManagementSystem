using LearningManagementSystem.Application.Abstractions.Services.Faculty;

namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public record MajorResponse(
    Guid Id,
    string Title,
    float Point,
    string EducationLanguage,
    decimal TuitionFee,
    bool StateFunded,
    FacultyResponse Faculty
);