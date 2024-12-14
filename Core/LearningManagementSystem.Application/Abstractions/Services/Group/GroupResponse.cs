using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.Services.Term;

namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public record GroupResponse(
    Guid Id,
    char Code,
    string Name,
    TimeSpan StartDate,
    TimeSpan EndDate,
    DayOfWeek DayOfWeek,
    TermResponse Term,
    TeacherResponse Teacher,
    SubjectResponse Subject,
    MajorResponse Major,
    List<LessonResponse> Lessons,
    List<ExamResponse> Exams
    );