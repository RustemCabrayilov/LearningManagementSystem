using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class VoteConfiguration:IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.Property(x => x.Point).IsRequired();
        builder.Property(x => x.QuestionId).IsRequired();
        builder.Property(x => x.StudentId).IsRequired();
        builder
            .HasOne(e => e.Survey) // assuming there's a navigation property in Exam for Group
            .WithMany(g => g.Votes)
            .HasForeignKey(e => e.SurveyId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(e => e.Question) // assuming there's a navigation property in Exam for Group
            .WithMany(g => g.Votes)
            .HasForeignKey(e => e.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}