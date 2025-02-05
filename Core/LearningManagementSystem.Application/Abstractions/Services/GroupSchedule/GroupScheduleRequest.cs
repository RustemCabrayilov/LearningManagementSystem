namespace LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;

public record GroupScheduleRequest(
    Guid GroupId,
    string ClassTime,
    DayOfWeek DayOfWeek);