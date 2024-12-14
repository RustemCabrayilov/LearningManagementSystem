namespace LearningManagementSystem.Application.Abstractions.Services.Auth;

public record SignUpRequest(
    string UserName,
    string Email,
    string Password,
    string ConfirmPassword,
    string PhoneNumber);