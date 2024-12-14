namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public record MajorResponse(
    Guid Id,
    string Name,
    float Point,
    string EducationLanguage,
    decimal TuitionFee,
    decimal StateFunded,
    Domain.Entities.Faculty Faculty
);