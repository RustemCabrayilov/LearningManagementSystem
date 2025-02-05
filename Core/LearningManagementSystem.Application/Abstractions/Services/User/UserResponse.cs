namespace LearningManagementSystem.Application.Abstractions.Services.User;

public record UserResponse(
    string Id,
    string UserName,
    string PhoneNumber,
    string Email,
    string ConnectionId=null,
    List<string> Roles=null
    );