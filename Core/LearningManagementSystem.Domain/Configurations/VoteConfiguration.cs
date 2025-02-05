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


    }
}