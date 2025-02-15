namespace LearningManagementSystem.Application.Abstractions.Services.Attendance;

public record StudentAttendanceDto(
    Guid StudentId,
    Guid GroupId
);