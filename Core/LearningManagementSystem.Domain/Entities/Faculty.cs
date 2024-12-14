using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Faculty:BaseEntity
{
    public string Name { get; set; }
    public List<Major> Majors { get; set; }
    public List<Teacher> Teachers { get; set; }
}