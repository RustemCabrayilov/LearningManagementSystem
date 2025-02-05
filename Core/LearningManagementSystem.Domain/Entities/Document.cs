using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(DocumentConfiguration))]
public class Document:BaseEntity
{
    public DocumentType DocumentType { get; set; }
    public string? Path { get; set; }
    public string Key { get; set; }
    public string FileName { get; set; }
    public string OriginName { get; set; }
    public Guid OwnerId { get; set; }
}