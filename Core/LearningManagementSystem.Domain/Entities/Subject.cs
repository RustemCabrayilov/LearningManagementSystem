using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; }
    public string SubjectCode { get; set; }
    public int AttendanceLimit { get; set; }
    public List<Lesson> Lessons { get; set; }
}