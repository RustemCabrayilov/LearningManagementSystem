using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningManagementSystem.Application.Abstractions.Services.Token;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LearningManagementSystem.Infrastructure.Services.Token;

public class TokenHandler : ITokenHandler
{
    private readonly TokenSettings _tokenSettings;
    private readonly UserManager<AppUser> _userManager;

    public TokenHandler(
        IOptions<TokenSettings> settings,
        UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _tokenSettings = settings.Value;
    }

    public async Task<Application.Abstractions.Services.Token.Token> CreateAccessToken(int minute, AppUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user?.UserName),
        };
        var roles=await _userManager.GetRolesAsync(user);
        // Add roles as claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiration = DateTime.UtcNow.AddMinutes(minute);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = _tokenSettings.Issuer,
            Audience = _tokenSettings.Audience,
            SigningCredentials = signingCredentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return new Application.Abstractions.Services.Token.Token()
        {
            AccessToken = tokenHandler.WriteToken(securityToken),
            Expiration = expiration
        };
    }
}