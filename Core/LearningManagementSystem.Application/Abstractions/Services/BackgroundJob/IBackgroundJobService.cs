namespace LearningManagementSystem.Application.Abstractions.Services.BackgroundJob;

public interface IBackgroundJobService
{
  Task Recommendteacher();
  Task AveragOfStudent();

}