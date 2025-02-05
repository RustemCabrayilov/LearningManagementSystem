using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(SurveyConfiguration))]
public class Survey : BaseEntity
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public Term Term { get; set; }
    public Guid TermId { get; set; }
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public List<Question> Questions { get; set; }
}