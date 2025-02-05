using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class QuestionConfiguration:IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.MaxPoint).IsRequired().HasMaxLength(200);
        builder.Property(x => x.SurveyId).IsRequired();
    }
}