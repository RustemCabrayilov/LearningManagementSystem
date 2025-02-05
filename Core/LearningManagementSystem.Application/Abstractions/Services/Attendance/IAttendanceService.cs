using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Persistence.Filters;

namespace LearningManagementSystem.Application.Abstractions.Services.Attendance;

public interface IAttendanceService
{
    Task<AttendanceResponse> CreateAsync(AttendanceRequest dto);
    Task<AttendanceResponse[]> UpdateRangeAsync(AttendanceRequest[] dtos);
    Task<AttendanceResponse> UpdateAsync(Guid id,AttendanceRequest dto);
    Task<AttendanceResponse> RemoveAsync(Guid id);
    Task<AttendanceResponse> GetAsync(Guid id);
    Task<IList<AttendanceResponse>> GetAllAsync(RequestFilter? filter);   
}