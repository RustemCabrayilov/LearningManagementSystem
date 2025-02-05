using System.Security.Claims;

namespace LearningManagementSystem.Application.Abstractions.Services.User;

public record UserClaim(
    string Id,
    List<string> Roles);