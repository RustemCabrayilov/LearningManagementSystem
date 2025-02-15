using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(TranscriptConfiguration))]
public class Transcript:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public float TotalPoint { get; set; }
}