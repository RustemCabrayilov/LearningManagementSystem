using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public record DeanRequest(
    string Name,
    string Surname,
    string Salary,
    PositionType PositionType,
    Guid FacultyId,
    string AppUserId
);