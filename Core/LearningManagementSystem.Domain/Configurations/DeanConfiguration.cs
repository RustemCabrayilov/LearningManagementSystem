using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class DeanConfiguration:IEntityTypeConfiguration<Dean>
{
    public void Configure(EntityTypeBuilder<Dean> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Salary).IsRequired();
        builder.Property(x => x.AppUserId).IsRequired();
        builder.Property(x => x.FacultyId).IsRequired();
        builder.Property(x => x.PositionType).IsRequired().HasConversion<string>();
    }
}