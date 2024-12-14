using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Teacher;

public record TeacherRequest(
    string Name,
    string Surname,
    string Occupation,
    decimal Salary,
    float Rate,
    string AppUserId,
    Guid FacultyId,
    IFormFile File
);