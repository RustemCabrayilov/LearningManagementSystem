namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public record MajorRequest(
    string Name,
    float Point,
    string EducationLanguage,
    decimal TuitionFee,
    decimal StateFunded,
    Guid FacultyId
);