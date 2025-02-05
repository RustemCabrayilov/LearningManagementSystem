using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class TermConfiguration:IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> builder)
    {
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        

        
        /*
        builder
            .HasMany(e => e.StudentRetakeExams)
            .WithOne(se => se.Term)
            .HasForeignKey(se => se.TermId)
            .OnDelete(DeleteBehavior.Restrict);  */
    }
}