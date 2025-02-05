using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Attendance;

public record AttendanceResponse(
    Guid Id,
    StudentResponse Student,
    LessonResponse Lesson,
    bool Absence);