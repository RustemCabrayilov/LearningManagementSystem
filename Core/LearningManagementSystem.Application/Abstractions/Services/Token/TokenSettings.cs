namespace LearningManagementSystem.Application.Abstractions.Services.Token;

public class TokenSettings
{
    public string Issuer { get; init; }

    public string Audience { get; init; }

    public string SecurityKey { get; init; }
}