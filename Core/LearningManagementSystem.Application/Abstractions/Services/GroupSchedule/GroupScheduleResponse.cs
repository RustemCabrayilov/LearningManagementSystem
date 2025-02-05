using LearningManagementSystem.Application.Abstractions.Services.Group;

namespace LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;

public record GroupScheduleResponse(
    Guid Id,
    GroupResponse Group,
    string ClassTime,
    DayOfWeek DayOfWeek
);