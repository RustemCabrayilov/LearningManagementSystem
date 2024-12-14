using LearningManagementSystem.Application.Abstractions.Services.Email;

namespace LearningManagementSystem.BLL.Services.Email;

public class EmailService:IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        throw new NotImplementedException();
    }
}