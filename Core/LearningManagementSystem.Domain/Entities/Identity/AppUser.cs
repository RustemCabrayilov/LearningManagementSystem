using Microsoft.AspNetCore.Identity;

namespace LearningManagementSystem.Domain.Entities.Identity;

public class AppUser:IdentityUser<string>
{
    public string? ConnectionId { get; set; }
}