using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class ExamGroupConfiguration:IEntityTypeConfiguration<ExamGroup>
{
    public void Configure(EntityTypeBuilder<ExamGroup> builder)
    {
        builder.HasOne(x => x.Exam)
            .WithMany(x => x.ExamGroups)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Group)
            .WithMany(x => x.ExamGroups)
            .HasForeignKey(x => x.ExamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}