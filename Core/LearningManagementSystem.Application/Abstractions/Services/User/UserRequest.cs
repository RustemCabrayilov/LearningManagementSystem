namespace LearningManagementSystem.Application.Abstractions.Services.User;

public record UserRequest(
    string UserName,
    string Password,
    string PhoneNumber,
    string Email
    );