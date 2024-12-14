using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(ExamConfiguration))]
public class Exam:BaseEntity
{
    public decimal MaxPoint { get; set; }
    public ExamType ExamType { get; set; }
    public Subject Subject { get; set; }
    public Guid SubjectId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<ExamGroup> ExamGroups { get; set; }
    public List<RetakeExam> RetakeExams { get; set; }
}