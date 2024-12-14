using LearningManagementSystem.Application.Abstractions.Services.Lesson;

namespace LearningManagementSystem.Application.Abstractions.Services.Subject;

public record SubjectResponse(
    Guid Id,
    string Name,
    string SubjectCode,
    int AttendanceLimit,
    List<LessonResponse> Lessons);