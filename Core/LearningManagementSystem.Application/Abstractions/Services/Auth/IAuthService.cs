namespace LearningManagementSystem.Application.Abstractions.Services.Auth;

public interface IAuthService
{
    Task SignUpAsync(SignUpRequest dto);
    Task SignInAsync(SignInRequest dto);
    Task ConfirmEmailAsync(string email,string token);
}