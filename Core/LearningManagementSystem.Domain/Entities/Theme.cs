using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Theme:BaseEntity
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
}