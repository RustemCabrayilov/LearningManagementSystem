using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Theme;

public record ThemeRequest(
    string Title,
    IFormFile File
    );