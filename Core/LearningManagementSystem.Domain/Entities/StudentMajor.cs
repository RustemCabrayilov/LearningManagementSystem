using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class StudentMajor:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Major Major { get; set; }
    public Guid MajorId { get; set; }
}