namespace LearningManagementSystem.Application.Abstractions.Services.Role;

public interface IRoleService
{
    Task<RoleResponse> CreateAsync(RoleRequest dto);
    Task<RoleResponse> UpdateAsync(string id,RoleRequest dto);
    Task<RoleResponse> RemoveAsync(string id);
    Task<RoleResponse> GetAsync(string id);
    Task<IList<RoleResponse>> GetAllAsync();   
}
