using System.Security.Claims;
using LearningManagementSystem.UI.Integrations;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context,ILearningManagementSystem _learningManagementSystem)
    {
        var token = context.Request.Cookies["access_token"];
        if (!string.IsNullOrEmpty(token))
        {
            // Call your backend to validate the token
            var userClaims = await _learningManagementSystem.GetUserInfosByToken(token);
            if (userClaims != null)
            {
                // Populate the User object
                var claims = new List<Claim>();
                claims.AddRange(userClaims.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var claimsIdentity = new ClaimsIdentity(claims, "Token");
                context.User = new ClaimsPrincipal(claimsIdentity);
            }
        }

        await _next(context);
    }
}