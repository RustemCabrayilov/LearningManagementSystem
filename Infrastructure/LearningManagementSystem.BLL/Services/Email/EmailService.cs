using System.Net;
using System.Net.Mail;
using System.Text;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.BLL.Services.Email;

public class EmailService:IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _emailSettings = settings.Value;
    }

    public async Task SendEmailAsync(string body,string subject, string toEmail)
    {
        SmtpClient client = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(_emailSettings.FromMail, _emailSettings.Password);

        // Create email message
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_emailSettings.FromMail);

        mailMessage.To.Add(toEmail);
        mailMessage.Subject = subject;
        mailMessage.IsBodyHtml = true;
        StringBuilder mailBody = new StringBuilder();

        mailBody.AppendFormat("<br />");
        mailBody.AppendFormat(body);
        mailMessage.Body = mailBody.ToString();

        // Send email
        await client.SendMailAsync(mailMessage);
    }

}