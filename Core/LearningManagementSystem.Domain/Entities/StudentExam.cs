using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class StudentExam:BaseEntity
{
    public Exam Exam { get; set; }
    public Guid ExamId { get; set; }
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public float Point { get; set; }
}