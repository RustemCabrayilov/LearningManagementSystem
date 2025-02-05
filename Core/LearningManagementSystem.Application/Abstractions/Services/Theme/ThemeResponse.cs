namespace LearningManagementSystem.Application.Abstractions.Services.Theme;

public record ThemeResponse(
    Guid Id,
    string Title,
    bool IsActive,
    string FileUrl=null
    );