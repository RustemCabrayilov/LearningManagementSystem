using LearningManagementSystem.Application.Abstractions.Services.Subject;

namespace LearningManagementSystem.Application.Abstractions.Services.Stats;

public interface IStatsService
{
     ValueTask<float> AverageTeacherRate();
     Task AveragOfStudent();
     Task<List<StatsResponse<SubjectResponse>>> MostFailedSubjects();
}