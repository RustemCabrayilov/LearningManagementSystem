using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Services.Token;

public interface ITokenHandler
{
    Task<Token> CreateAccessToken(int minute,AppUser user);
}