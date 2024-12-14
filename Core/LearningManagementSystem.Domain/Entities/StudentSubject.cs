using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Domain.Entities;

public class StudentSubject:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Subject Subject { get; set; }
    public Guid SubjectId { get; set; }
    public int TotalPoint { get; set; }
    public SubjectTakingType SubjectTakingType { get; set; }
}