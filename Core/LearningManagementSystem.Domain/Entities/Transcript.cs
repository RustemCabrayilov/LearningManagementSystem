using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Transcript:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public float TotalPoint { get; set; }
}