using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class SubjectMajor:BaseEntity
{
    public Subject Subject { get; set; }
    public Guid SubjectId { get; set; }  
    public Major Major { get; set; }
    public Guid MajorId { get; set; }
    public int Credit { get; set; }
}