using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Lesson;

public record LessonResponse(
    Guid Id,
    string Name,
    GroupResponse Group,
    List<AttendanceResponse>? Attendances
);