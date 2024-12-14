using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class StudentGroup:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Group Group { get; set; }
    public Guid GroupId { get; set; }   
    public int AbsenceCount { get; set; }
    public int AttendanceCount { get; set; }
    public float ActivityPoint { get; set; }
    public float AttendancePoint { get; set; }
}