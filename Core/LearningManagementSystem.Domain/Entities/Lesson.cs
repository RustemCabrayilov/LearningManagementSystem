using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Lesson : BaseEntity
{
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public List<StudentLesson> StudentLessons { get; set; }
}