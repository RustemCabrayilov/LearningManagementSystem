using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class MajorConfiguration:IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Point).IsRequired();
        builder.Property(x => x.FacultyId).IsRequired();
        builder.Property(x => x.EducationLanguage).IsRequired().HasMaxLength(250);
        builder.Property(x => x.TuitionFee).IsRequired();
    }
}