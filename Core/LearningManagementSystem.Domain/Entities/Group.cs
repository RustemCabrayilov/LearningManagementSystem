using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Group:BaseEntity
{
    public char Code { get; set; }
    public string Name { get; set; }
    public TimeSpan StartDate { get; set; }
    public TimeSpan EndDate { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public Term Term { get; set; }   
    public Guid TermId { get; set; }
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public Subject Subject { get; set; }
    public Guid SubjectId { get; set; }
    public Major Major { get; set; }
    public Guid MajorId { get; set; }
    public List<Lesson> Lessons { get; set; }
    public List<ExamGroup> ExamGroups { get; set; }
}