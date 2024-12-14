namespace LearningManagementSystem.Application.Abstractions.Services.Auth;

public record SignInRequest(
    string UserName, string Password
    ,bool IsPersistent);