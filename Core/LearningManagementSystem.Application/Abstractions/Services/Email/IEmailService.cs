namespace LearningManagementSystem.Application.Abstractions.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(string body, string subject, string to);
}