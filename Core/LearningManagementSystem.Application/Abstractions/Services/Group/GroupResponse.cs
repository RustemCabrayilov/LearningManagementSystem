using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.Services.Term;

namespace LearningManagementSystem.Application.Abstractions.Services.Group;

public record GroupResponse(
    Guid Id,
    char Code,
    string Name,
    int Credit,
    bool CanApply,
    TermResponse Term,
    TeacherResponse Teacher,
    SubjectResponse Subject,
    MajorResponse Major,
    List<GroupScheduleResponse> GroupSchedules = null,
    List<LessonResponse> Lessons = null,
    List<StudentResponse> Students = null,
    List<ExamResponse> Exams = null);