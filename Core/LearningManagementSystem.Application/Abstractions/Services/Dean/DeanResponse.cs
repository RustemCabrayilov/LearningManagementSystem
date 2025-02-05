using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public record DeanResponse(
    Guid Id,
    string Name,
    string Surname,
    decimal Salary,
    PositionType PositionType,
    FacultyResponse Faculty,
    AppUser AppUser,
    string QrCodeUrl="",
    string FileUrl=null
    );