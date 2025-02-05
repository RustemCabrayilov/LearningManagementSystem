using LearningManagementSystem.Application.Abstractions.Services.Role;

namespace LearningManagementSystem.Application.Abstractions.Services.User;

public record UserRoleDto(
    string UserId,
    string RoleName,
    IList<RoleResponse> Roles=null);