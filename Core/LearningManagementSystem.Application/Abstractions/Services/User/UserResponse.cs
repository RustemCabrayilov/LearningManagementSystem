namespace LearningManagementSystem.Application.Abstractions.Services.User;

public record UserResponse(
    Guid Id,
    string UserName,
    string PhoneNumber,
    string Email,
    List<string> Roles=null
    );