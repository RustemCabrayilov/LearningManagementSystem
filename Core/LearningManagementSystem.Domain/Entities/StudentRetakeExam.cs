using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class StudentRetakeExam:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public RetakeExam RetakeExam { get; set; }
    public Guid RetakeExamId { get; set; }
}