using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Persistence.Context;

namespace LearningManagementSystem.Persistence.UnitOfWork;

public class UnitOfWork(AppDbContext _dbContext): IUnitOfWork
{
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}