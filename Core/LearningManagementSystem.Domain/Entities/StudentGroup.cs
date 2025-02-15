using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(StudentGroupConfiguration))]
public class StudentGroup:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }   
    public int AbsenceCount { get; set; }
    public int AttendanceCount { get; set; }
}