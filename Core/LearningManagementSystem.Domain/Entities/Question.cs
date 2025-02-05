using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(QuestionConfiguration))]
public class Question:BaseEntity
{
    public string Description { get; set; }
    public int MaxPoint { get; set; }
    public Survey Survey { get; set; }
    public Guid SurveyId { get; set; }
}