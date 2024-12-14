using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Domain.Entities;

public class StudentLesson:BaseEntity
{
    public Student Student { get; set; }
    public Guid StudentId { get; set; }
    public Lesson Lesson { get; set; }
    public Guid LessonId { get; set; }
    public bool Absence { get; set; }
    public bool Attendance { get; set; }
}