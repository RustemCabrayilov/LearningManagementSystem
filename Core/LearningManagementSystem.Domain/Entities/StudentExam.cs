using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(StudentExamConfiguration))]

public class StudentExam:BaseEntity
{
    public Exam Exam { get; set; }
    public Guid ExamId { get; set; }
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
 
    public float Point { get; set; }
}