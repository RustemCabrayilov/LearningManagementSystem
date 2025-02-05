namespace LearningManagementSystem.Application.Abstractions.Services.Attendance;

public record AttendanceRequest(
    Guid Id,
    Guid StudentId,
    Guid LessonId,
    bool Absence
);