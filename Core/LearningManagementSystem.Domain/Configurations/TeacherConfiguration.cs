using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class TeacherConfiguration: IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Salary).IsRequired();
        builder.Property(x => x.AppUserId).IsRequired();
        builder.Property(x => x.FacultyId).IsRequired();
        builder.HasMany(t => t.Groups)
            .WithOne(g => g.Teacher)
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(t => t.Groups)
            .WithOne(g => g.Teacher)
            .HasForeignKey(g => g.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}