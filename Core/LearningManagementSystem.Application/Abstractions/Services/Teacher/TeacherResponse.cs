using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Teacher;

public record TeacherResponse(
    Guid Id,
    string Name,
    string Surname,
    string Occupation,
    decimal Salary,
    float Rate,
    AppUser AppUser,
    Domain.Entities.Faculty Faculty,
    IFormFile? File
    );