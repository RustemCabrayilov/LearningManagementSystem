namespace LearningManagementSystem.Application.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    void SaveChanges();
    Task SaveChangesAsync();
}