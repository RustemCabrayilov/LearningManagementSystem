using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Lesson;

public record LessonResponse(
    Guid Id,
    GroupResponse Group,
    List<StudentResponse> Students);