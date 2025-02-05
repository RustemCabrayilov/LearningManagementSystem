using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public record DeanRequest(
    string Name,
    string Surname,
    decimal Salary,
    PositionType PositionType,
    string FacultyId,
    string AppUserId,
    IFormFile File
);