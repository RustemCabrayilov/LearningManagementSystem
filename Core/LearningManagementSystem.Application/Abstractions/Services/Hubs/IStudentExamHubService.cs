namespace LearningManagementSystem.Application.Abstractions.Services.Hubs;

public interface IStudentExamHubService
{
    Task StudentExamAddedService(Guid studentId, string message);
}