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
        builder.Property(x => x.GroupId).IsRequired();
        builder
            .HasOne(e => e.Group) // assuming there's a navigation property in Exam for Group
            .WithMany(g => g.Exams)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        /*builder
            .HasMany(e => e.StudentExams)
            .WithOne(se => se.Exam)
            .HasForeignKey(se => se.ExamId)
            .OnDelete(DeleteBehavior.Restrict);  */
          
        
    }
}