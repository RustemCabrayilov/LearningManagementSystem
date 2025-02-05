using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class RetakeExamConfiguration:IEntityTypeConfiguration<RetakeExam>
{
    public void Configure(EntityTypeBuilder<RetakeExam> builder)
    {
        builder.Property(x=>x.Price).IsRequired();
        builder.Property(x=>x.ApplyDate).IsRequired();
        builder.Property(x=>x.ExamId).IsRequired();
        builder.Property(x=>x.Deadline).IsRequired();
        builder
            .HasMany(e => e.StudentRetakeExams)
            .WithOne(se => se.RetakeExam)
            .HasForeignKey(se => se.RetakeExamId)
            .OnDelete(DeleteBehavior.Restrict);  
    }
}