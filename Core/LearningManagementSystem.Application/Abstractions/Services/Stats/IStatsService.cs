using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;

namespace LearningManagementSystem.Application.Abstractions.Services.Stats;

public interface IStatsService
{
     Task<List<StatsResponse<TeacherResponse>>> TopTeacherRate();
     Task<List<StatsResponse<StudentResponse>>> AveragOfStudent();
     Task<List<StatsResponse<SubjectResponse>>> MostFailedSubjects();
     Task<List<StatsResponse<TeacherResponse>>> MostEfficientTeachers();
}