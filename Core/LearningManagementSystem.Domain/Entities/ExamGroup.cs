using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(ExamGroupConfiguration))]
public class ExamGroup:BaseEntity
{
    public Exam Exam { get; set; }
    public Guid ExamId { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }   
}