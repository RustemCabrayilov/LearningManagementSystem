using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.User;

public interface IUserService
{
    Task<UserResponse> CreateAsync(UserRequest dto);
    Task<UserResponse> UpdateAsync(string id,UserRequest dto);
    Task<UserResponse> RemoveAsync(string id);
    Task<UserResponse> GetAsync(string id);
    Task<IList<UserResponse>> GetAllAsync();   
    Task<UserResponse> AssignRoleAsync(UserRoleDto dto);   
}