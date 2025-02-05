using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class StudentRetakeExamConfiguration:IEntityTypeConfiguration<StudentRetakeExam>
{
    public void Configure(EntityTypeBuilder<StudentRetakeExam> builder)
    {
        /*builder
            .HasOne(e => e.Student) // assuming there's a navigation property in Exam for Group
            .WithMany(g => g.StudentRetakeExams)
            .HasForeignKey(e => e.RetakeExamId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(e => e.Term) // assuming there's a navigation property in Exam for Group
            .WithMany(g => g.StudentRetakeExams)
            .HasForeignKey(e => e.TermId)
            .OnDelete(DeleteBehavior.Restrict);*/
    }
}