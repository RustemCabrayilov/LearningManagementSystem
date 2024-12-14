using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentResponse(
    Guid Id,
    string Name,
    string Surname,
    string StudentNo,
    AppUser AppUser
); 