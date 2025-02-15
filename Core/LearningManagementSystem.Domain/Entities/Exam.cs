using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(ExamConfiguration))]
public class Exam:BaseEntity
{
    public string Name { get; set; }
    public decimal MaxPoint { get; set; }
    public ExamType ExamType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public List<RetakeExam> RetakeExams { get; set; }
    public List<StudentExam> StudentExams { get; set; }
}