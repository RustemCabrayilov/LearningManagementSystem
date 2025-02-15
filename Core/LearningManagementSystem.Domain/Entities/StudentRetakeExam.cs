using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(StudentRetakeExamConfiguration))]
public class StudentRetakeExam:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public RetakeExam RetakeExam { get; set; }
    public Guid RetakeExamId { get; set; }
    public DateTime ApplyDate { get; set; }=DateTime.Now;
    public Status Status { get; set; }
    public float NewPoint { get; set; }
}