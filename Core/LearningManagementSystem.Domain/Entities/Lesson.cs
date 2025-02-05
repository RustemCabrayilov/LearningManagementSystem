using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(LessonConfiguration))]
public class Lesson : BaseEntity
{
    public string Name { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public List<Attendance> Attendances { get; set; }
}