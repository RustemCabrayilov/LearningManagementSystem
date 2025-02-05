namespace LearningManagementSystem.Application.Abstractions.Services.Auth;

public interface IAuthService
{
    Task<Token.Token> SignUpAsync(SignUpRequest dto);
    Task<Token.Token> SignInAsync(SignInRequest dto);
    Task ConfirmEmailAsync(string email,string token);
}