using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class GroupExam:BaseEntity
{
    public Exam Exam { get; set; }
    public Guid ExamId { get; set; }
    public Student Group { get; set; }
    public Guid GroupId { get; set; }
}