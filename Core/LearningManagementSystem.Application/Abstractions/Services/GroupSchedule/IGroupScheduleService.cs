using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;

public interface IGroupScheduleService
{
    Task<GroupScheduleResponse> CreateAsync(GroupScheduleRequest dto);
    Task<GroupScheduleResponse> UpdateAsync(Guid id,GroupScheduleRequest dto);
    Task<GroupScheduleResponse> RemoveAsync(Guid id);
    Task<GroupScheduleResponse> GetAsync(Guid id);
    Task<IList<GroupScheduleResponse>> GetAllAsync(RequestFilter? filter);
}