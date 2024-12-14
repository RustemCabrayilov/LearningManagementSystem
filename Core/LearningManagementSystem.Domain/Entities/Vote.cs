using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(VoteConfiguration))]
public class Vote:BaseEntity
{
    public string Description { get; set; }
    public int Point { get; set; }
    public Survey Survey { get; set; }
    public Guid SurveyId { get; set; }
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
}