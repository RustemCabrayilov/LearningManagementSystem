using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Student;

public record StudentRequest(
    string AppUserId,
    string Name,
    string Surname,
    string StudentNo
    );