using LearningManagementSystem.Application.Abstractions.Services.Faculty;

namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public record MajorRequest(
    string Title,
    float Point,
    string EducationLanguage,
    decimal TuitionFee,
    bool StateFunded,
    Guid FacultyId);