using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentRequest(
    string AppUserId,
    string Name,
    string Surname,
    string StudentNo,
    IFormFile File
    );