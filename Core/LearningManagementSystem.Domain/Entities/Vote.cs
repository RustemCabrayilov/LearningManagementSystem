using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(VoteConfiguration))]
public class Vote:BaseEntity
{
    public int Point { get; set; }
    public Question Question { get; set; }
    public Guid QuestionId { get; set; }
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Survey Survey { get; set; }
    public Guid SurveyId { get; set; }
}