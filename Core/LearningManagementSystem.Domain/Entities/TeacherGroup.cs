using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class TeacherGroup:BaseEntity
{
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public Subject Group { get; set; }
    public Guid GroupId { get; set; }
}