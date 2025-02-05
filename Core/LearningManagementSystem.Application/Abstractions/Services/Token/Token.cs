namespace LearningManagementSystem.Application.Abstractions.Services.Token;

public class Token
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}