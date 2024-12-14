using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class ExamConfiguration:IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.Property(x => x.ExamType).IsRequired().HasConversion<string>();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.SubjectId).IsRequired();
    }
}