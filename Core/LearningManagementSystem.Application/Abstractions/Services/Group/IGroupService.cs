using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public interface IGroupService
{
    Task<GroupResponse> CreateAsync(GroupRequest dto);
    Task<GroupResponse> UpdateAsync(Guid id,GroupRequest dto);
    Task<GroupResponse> RemoveAsync(Guid id);
    Task<GroupResponse> GetAsync(Guid id);
    Task<IList<GroupResponse>> GetAllAsync(RequestFilter? filter);
    Task<GroupResponse> Activate(Guid id);

}