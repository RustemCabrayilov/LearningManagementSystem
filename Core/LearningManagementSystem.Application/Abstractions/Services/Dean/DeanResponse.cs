using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public class DeanResponse(
    Guid Id,
    string Name,
    string Surname,
    string Salary,
    PositionType PositionType,
    FacultyResponse Faculty,
    AppUser AppUser
    );