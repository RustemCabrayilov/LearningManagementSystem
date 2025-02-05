using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearningManagementSystem.Domain.Configurations;

public class DocumentConfiguration:IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.Property(x => x.FileName).IsRequired();
        builder.Property(x => x.OriginName).IsRequired();
        builder.Property(x => x.OwnerId).IsRequired();
    }
}